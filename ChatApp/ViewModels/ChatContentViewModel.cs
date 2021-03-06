﻿using ChatApp.Models;
using ChatApp.ViewModels.Commands;

using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace ChatApp.ViewModels
{
    interface IChatContentViewModel { }

    class TextContentViewModel : AbstractViewModel, IChatContentViewModel
    {
        public string Content { get; private set; }

        public TextContentViewModel(TextContent content)
        {
            Content = content.Value;
        }
    }

    class DataContentViewModel : AbstractViewModel, IChatContentViewModel
    {
        private DataContent _content;

        public string FileName { get; private set; }
        public RelayCommand SaveFileCommand { get; private set; }

        public DataContentViewModel(DataContent content)
        {
            _content = content;
            FileName = content.FileName;

            SaveFileCommand = new RelayCommand(o => OpenFile());
        }
        public string SaveFile(string saveDirPath)
        {
            var saveFilePath = Path.Combine(saveDirPath, FileName);
            using (var fs = new FileStream(saveFilePath, FileMode.Create, FileAccess.Write))
            {
                using (var writer = new BinaryWriter(fs))
                {
                    writer.Write(_content.Value);
                }
            }
            return saveFilePath;
        }
        public void OpenFile()
        {
            var tempPath = SaveFile(Path.GetTempPath());
            System.Diagnostics.Process.Start(tempPath);
        }
    }

    class ImageContentViewModel : DataContentViewModel
    {
        public BitmapImage Image { get; private set; }

        public ImageContentViewModel(ImageContent content)
            : base(content)
        {
            Image = content.Image;
        }
    }   
}
