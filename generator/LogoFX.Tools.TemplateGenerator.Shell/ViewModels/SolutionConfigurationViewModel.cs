using System;
using System.IO;
using System.Windows.Input;
using Avalon.Windows.Dialogs;
using LogoFX.Tools.TemplateGenerator.Contracts;
using Microsoft.Expression.Interactivity.Core;

namespace LogoFX.Tools.TemplateGenerator.Shell.ViewModels
{
    public class SolutionConfigurationViewModel : ObjectViewModel<ISolutionConfiguration>
    {
        public SolutionConfigurationViewModel(ISolutionConfiguration model) 
            : base(model)
        {
        }

        private ICommand _browseDestinationFolderCommand;

        public ICommand BrowseDestinationFolderCommand
        {
            get
            {
                return _browseDestinationFolderCommand ??
                       (_browseDestinationFolderCommand = new ActionCommand(() =>
                       {
                           FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
                           {
                               RootSpecialFolder = Environment.SpecialFolder.MyComputer,
                               SelectedPath = DestinationPath,
                               Title = "Destination Path"
                           };

                           var retVal = folderBrowserDialog.ShowDialog() ?? false;

                           if (!retVal)
                           {
                               return;
                           }

                           DestinationPath = folderBrowserDialog.SelectedPath;
                       }));
            }
        }

        public string SolutionFileName => Model.FileName;

        public string DestinationPath
        {
            get { return Model.DestinationPath; }
            set
            {
                if (value == Model.DestinationPath)
                {
                    return;
                }

                Model.DestinationPath = value;
                NotifyOfPropertyChange();
            }
        }

        public string Name
        {
            get { return Model.Name; }
            set
            {
                if (value == Model.Name)
                {
                    return;
                }

                Model.Name = value;
                NotifyOfPropertyChange();
            }
        }

        public string Description
        {
            get { return Model.Description; }
            set
            {
                if (value == Model.Description)
                {
                    return;
                }

                Model.Description = value;
                NotifyOfPropertyChange();
            }
        }

        public string DefaultName
        {
            get { return Model.DefaultName; }
            set
            {
                if (value == Model.DefaultName)
                {
                    return;
                }

                Model.DefaultName = value;
                NotifyOfPropertyChange();
            }
        }

        public bool CanGenerate
        {
            get
            {
                if (string.IsNullOrWhiteSpace(DestinationPath))
                {
                    return false;
                }

                if (string.IsNullOrWhiteSpace(Name))
                {
                    return false;
                }

                if (string.IsNullOrWhiteSpace(DefaultName))
                {
                    return false;
                }

                if (!Directory.Exists(DestinationPath))
                {
                    return false;
                }

                return true;
            }
        }
    }
}