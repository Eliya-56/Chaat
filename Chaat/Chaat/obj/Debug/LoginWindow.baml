   M S B A M L   `   `   `  ����@  <Chaat, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:ChaatChaat  5   	       Chaat.LoginWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   %localclr-namespace:Chaat     5   	     � Title$
  Auth��5   	   $	��500��6   $	��300��6#    � 
ResizeMode$ CanMinimize=�6/   
�������.��5	      �  � RowDefinitions 5
   
   �� 5      $
��0.5* �6   6,   �� 5      $��* �6   6)   �� 5      $
��0.5* �6   6,   .+�5   
   �� $��1��6   .+�5      �� $��	15 0 15 0q�6   $��Center=�6+   $��15(�6H   Login6]   �� 5      -   5         ��Name$ 	LoginText��5   (   $��0 5 0 0 q�6   $��Center=�6;   $	��200��6X   $��18(�6e   6u   �� 5      $��	15 0 15 0q�6   $��Center=�6+   $��15(�6H   
Password6`   G� 5      -   5         G�Name$ PasswordText��5   +    G� PasswordChar$ *��6   $	��200��6A   $��18(�6N   6^   � 5       � ColumnDefinitions 5      �� 5      6)   �� 5      6)   .+�5      �� -   5         ��Name$ Up��5   )   $��0��6   $��	0 15 15 0q�65   $��Right=�6H   $��70��6d   $��20��6o   .��6�   SignUp6�   �� 5      -   5        $ In��5   )   $��1��6   $��	15 15 0 0q�65   $
��Left=�6H   $��70��6c   $��20��6n   .��6�   SignIn6�   5      �� 5      -   5         ��Name$ ErrorMessage��5   ,   $��
15 15 15 0q�6   $��Center=�6B   $��15(�6_   
���  ��6�   5      5      5      