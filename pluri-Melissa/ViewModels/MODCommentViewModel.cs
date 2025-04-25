using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Project.Models;
using Project.Services;

namespace Project.ViewModels
{
    public class MODCommentViewModel : ViewModelBase
    {
 
        //fields
        public string Username { get; set; }
        public string CommentText { get; set; }
        public int TheseId { get; set; }
        public ObservableCollection<Comment> FlaggedComments { get; set; }

        //commands
        public ICommand ApproveCommand { get; set; }
        public ICommand DenyCommand { get; set; }


        //contructor

           

            public MODCommentViewModel(IWindowManager windowManager, ViewModelLocator viewModelLocator)
            {
                FlaggedComments = new ObservableCollection<Comment>();

                // Example
                FlaggedComments.Add(new Comment
                {
                    Username = "usertest",
                    CommentText = "This is a negative comment!",
                    TheseId = 1,

                });

            ApproveCommand = new ViewModelCommand(
            execute: obj =>
            {
                

                
            }
            // canExecute: obj =>  !string.IsNullOrWhiteSpace(InputVerificationCode) && InputVerificationCode.Length >= 6
            );






            DenyCommand = new ViewModelCommand(
            execute: obj =>
            {



            }
            // canExecute: obj =>  !string.IsNullOrWhiteSpace(InputVerificationCode) && InputVerificationCode.Length >= 6
            );


        }

    }


}

