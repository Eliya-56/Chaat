   M S B A M L   `   `   `  ���� @  <Chaat, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:ChaatChaat      Chaat.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     %localclr-namespace:Chaat       � Title$  
MainWindow��$	��460��$	��670��.���  � RowDefinitions �� $
��0.1* ��� $
��0.7* ��� $��0.15* �.+��� -    ��Name$ 
TextScroll��$��1��$��Disabled=�$
��Auto=�.���� -    ��Name$ Messages�����#D3FDD4� $��2�� � ColumnDefinitions �� $
��0.9* ��� $
��0.1* �.+�]� 
��� � �.+��� -    ��Name$ UserText��$��Bottom=�$��7 7 7 7q�
��� �|� �� 	MaxLength$
 1000��$��14(� �� TextWrapping$
 Wrap=��� -    ��Name$ SendMessage��$��1��
������	 �� 	IsDefault	 . .��Send�� -   
 ��Name$
 ActiveUsers��
��� � �$��0q�$��20(�