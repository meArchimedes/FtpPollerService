using System.Data;
using Renci.SshNet;
using FtpPoller.Data;
using FtpPollerService.DTOs;

namespace FtpPollerService
{
    public class PollerFunctions
    {
        SftpClient _sClient, _dClient;
        List<List<string>> payersFiles = new List<List<string>>();
        private readonly ILogger<Worker> _logger;
        private readonly FtpPollerContext _DbContext;
        private PayerDTO[] payers;
        public PollerFunctions(FtpPollerContext ftpPollerContext, ILogger<Worker> logger)
        {
            _DbContext = ftpPollerContext;
            _logger = logger;
            foreach (var p in _DbContext.PayerServers) payersFiles.Add(new List<string>());
        }

        public async Task TransferFilesAsync()
        {
            try
            {
                payers = new PayerDTO[_DbContext.PayerServers.Count()];
                var tempPayers = _DbContext.PayerServers.ToList();
                for (int i = 0; i < _DbContext.PayerServers.Count(); i++)
                {
                    //Initialize payers previously copied files on start
                    if (payersFiles[i].Count == 0)
                    {
                        var copiedFiles = _DbContext.CopiedFiles.Where(f => f.PayerId == tempPayers[i].Id).Select(f => f.FileName).ToList();
                        payersFiles[i].AddRange(copiedFiles);
                    }

                    _sClient = new SftpClient(tempPayers[i].Host, tempPayers[i].Port, tempPayers[i].UserName, tempPayers[i].Password);
                    _sClient.Connect();
                    var payer = tempPayers[i];
                    // _sClient.ChangeDirectory(tempPayers[i].FolderDir);
                    var fileNames = _sClient
                        .ListDirectory(tempPayers[i].FolderDir)
                        .Where(file => !payersFiles[i].Contains(file.Name) && !file.IsDirectory && file.Name.EndsWith(".era"))
                        .Select(file => file.Name)
                        .ToList();

                    var payerDTO = new PayerDTO
                    {
                        Id = payer.Id,
                        SrcServer = payer.Host,
                        SrcPort = payer.Port,
                        SrcUserName = payer.UserName,
                        SrcPassword = payer.Password,
                        FolderDir = payer.FolderDir.Equals(null) ? "" : payer.FolderDir,
                        FilePaths = fileNames,
                    };
                    payers[i] = payerDTO;

                }

                // Check if there are no new files
                if (!payers.Any(p => p.FilePaths.Count > 0)) return;

                for (int i = 0; i < _DbContext.PayerServers.Count(); i++)
                {
                    //Check if files were added for current client
                    if (payers[i].FilePaths == null) break;

                    _logger.LogInformation($"{payers[i].FilePaths.Count} new era files detected for client {payers[i].SrcUserName}.");
                    await CopyNewFilesToRecipientFolders(payers[i], payers[i].Id);

                    await AddFilesToDB(payers[i].FilePaths, payers[i].Id);
                    payersFiles[i].AddRange(payers[i].FilePaths);
                }
                _DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured! \n{ex.Message}");
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                if (_sClient.IsConnected)
                    _sClient.Disconnect();

                if (_dClient.IsConnected)
                    _dClient.Disconnect();
            }
        }


        private async Task CopyNewFilesToRecipientFolders(PayerDTO payer, Guid payerId)
        {
            var subscribedFolders = _DbContext.Subscriptions.Where(s => s.PayerId == payerId).Select(s => s.RecipientId).ToList();
            var recipients = _DbContext.RecipientServers.Where(rs => subscribedFolders.Contains(rs.Id)).ToList();
            foreach (var recipient in recipients)
            {
                _dClient = new SftpClient(recipient.Host, recipient.Port, recipient.UserName, recipient.Password);

                _dClient.Connect();
                // Upload files to the working directory
                foreach (var filePath in payer.FilePaths)
                {
                    string fullFilePath = payer.FolderDir == null ? filePath : payer.FolderDir + "/" + filePath;
                    var fileInfo = _sClient.Get(fullFilePath);

                    using (Stream sourceStream = _sClient.OpenRead(fullFilePath))
                    {
                        byte[] fileContents;
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            sourceStream.CopyTo(memoryStream);
                            fileContents = memoryStream.ToArray();
                        }
                        using (Stream destStream = _dClient.OpenWrite(recipient.FolderDir + "/" + filePath))
                        {
                            destStream.Write(fileContents, 0, fileContents.Length);
                        }
                    }

                    _logger.LogInformation($"File {filePath} added to destination folder successfully.");
                }
            }

        }

        private async Task AddFilesToDB(List<string> files, Guid clientId)
        {
            FtpPoller.Data.Entities.File f;
            foreach (var file in files)
            {
                f = new FtpPoller.Data.Entities.File()
                {
                    FileName = file,
                    Id = new Guid(),
                    InsertedAt = DateTime.Now.ToString(),
                    PayerId = clientId
                };
                _DbContext.CopiedFiles.Add(f);
            }
        }

    }
}
