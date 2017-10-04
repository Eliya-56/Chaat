using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Chaat
{
    class MessagesBox
    {
        public string UserName { get; set; }
        public StackPanel Messages { get; set; }
        public bool IsActive { get; set; }

        public MessagesBox(string UserName, StackPanel parent)
        {
            this.UserName = UserName;
            IsActive = false;
            Messages = new StackPanel();
            Messages.Background = new SolidColorBrush(Color.FromRgb(0xD3, 0xFD, 0xD4));
            Messages.Visibility = System.Windows.Visibility.Collapsed;
            parent.Children.Add(Messages);
        }

        public void Active()
        {
            Messages.Visibility = System.Windows.Visibility.Visible;
            IsActive = true;
        }

        public void DeActive()
        {
            Messages.Visibility = System.Windows.Visibility.Collapsed;
            IsActive = false;
        }
        
        public override string ToString()
        {
            return UserName;
        }
    }
}
