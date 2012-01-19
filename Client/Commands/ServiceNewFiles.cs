using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Autofac;
using Client.Utils;
using Common.Files;

namespace Client.Commands
{
    public class ServiceNewFiles : ICommand
    {
        private readonly IEnumerable<File> _files;
        private readonly IContainer _container;

        public ServiceNewFiles(
            IEnumerable<File> files,
            IContainer container)
        {
            _files = files;
            _container = container;
        }

        public void Execute()
        {
            foreach (var file in _files)
            {
                if (MessageBoxService.ShowQuestion(
                    string.Format("Do you want to download file {0} from user {1}?", 
                    file.OriginalName, file.SenderNumber)) != MessageBoxResult.Yes)
                {
                    continue;
                }

                ICommand command = _container.Resolve<DownloadFile>(new UniqueTypeParameter(file));
                
            }
        }
    }
}
