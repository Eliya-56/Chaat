   M S B A M L   `   `   `  ����@  <Chaat, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:ChaatChaat  5   	       Chaat.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   %localclr-namespace:Chaat     5   	     � Title$  
MainWindow��5	   	   $	��460��6   $	��670��6)   .��5
      �  � RowDefinitions 5   
   �� 5      $
��0.1* �6   6,   �� 5      $
��0.7* �6   6,   �� 5      $��0.15* �6   6-   .+�5   
   �� -   5         ��Name$ 
TextScroll��5      $��1��6+   $��Disabled=�68   $
��Auto=�6c   .��5      �� -   5         ��Name$ Messages��5   .   ���#D3FDD45      5      � 5   
   $��2��6    � ColumnDefinitions 5      �� 5      $
��0.9* �6#   62   �� 5      $
��0.1* �6#   62   .+�5      ]� 
��� � �.+�5      �� -   5         ��Name$ UserText��5   2   $��Bottom=�6   $��7 7 7 7q�6D   
��� �|� �� 	MaxLength$
 1000��6l   $��14(�6}    �� TextWrapping$
 Wrap=�6�   6�   5      �� 5      -   5         ��Name$ SendMessage��5   %   $��1��6   
������	 �� 	IsDefault	 . .��6}   Send6�   5      �� 5   
   -   5        
 ��Name$
 ActiveUsers��5   :   
��� � �$��0q�6&   $��20(�6O   6�   5       5!      