   M S B A M L   `   `   `  ����  MessengerClient     MessengerClient.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  
MainWindow��5   	   $	��550��6   $	��525��6)    � 
ResizeMode$ 	CanResize=�5   '    � Icon$ Icons/Chat.ico��5   	   .��5      �  � RowDefinitions 5	   
   �� 5
      $��* �6   6)   �� 5      $��70 �6   6*    � ColumnDefinitions 5   
   �� 5      $��* �6   6+   �� 5      $	��100 �6   6-   .+�5   
   (  "System.Windows.Controls.WebBrowser  -   5        $��WebBrowserArea��5   6   $��0��6   $��2��6"   $��10q�6M   6�   �� 5   
   -   5        $��MessageTextBox��5   3   $��1��6   $��0��6#   $��	10,0,0,20q�6I   " �� VerticalScrollBarVisibility$
 Auto=�6}   6�   �� 5   
   -   5        $��
SendButton��5   .   $��1��6   $��1��6   $��Отправить��6@   $��	0,0,10,20q�6U   
����� �6�   5      5      