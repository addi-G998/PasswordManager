   M S B A M L   `   `   `  ����L  HPasswordMng_v0.01, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:PasswordMng_v0._01PasswordMng_v0._01  5   	       PasswordMng_v0._01.Editor   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:PasswordMng_v0._01     5   	     � Title$  Editor��5   	   $	��358��6   $	��619��6%    � 
ResizeMode$ NoResize=�61    � WindowStyle$
 None=�6G   .��5	      � .+�5
   
   �� -   5         ��Name$ idBox��5
      $
��Left=�6!   $��
348,56,0,0q�6<    �� TextWrapping$
 Wrap=�6P   $��TextBox��6d   $	��Top=�6s   $	��242��6�   $��33��6�   �� 5   
   -   5        $ userBox��5      $
��Left=�6#   $��348,146,0,0q�6>   $
 Wrap=�6S   $��TextBox��6g   $	��Top=�6v   $	��242��6�   $��33��6�   �� 5   
   -   5        $ passBox��5      $
��Left=�6#   $��348,234,0,0q�6>   $
 Wrap=�6S   $��TextBox��6g   $	��Top=�6v   $	��242��6�   $��33��6�   �� 5   
   $��ID��6   $
��Left=�6   $��
145,56,0,0q�68   $	��Top=�6L   $��33��6d   $��88��6p   $��16(�6{   $
��Bold"�6�   �� 5   
   $��Username��6   $
��Left=�6#   $��145,146,0,0q�6>   $	��Top=�6S   $��32��6k   $��88��6w   $��16(�6�   $
��Bold"�6�   �� 5   
   $��Password��6   $
��Left=�6#   $��145,235,0,0q�6>   $	��Top=�6S   $��32��6k   $��88��6w   $��16(�6�   $
��Bold"�6�   �� 5   
   -   5         ��Name$ 
confirmBtn��5      
���   �$��1q�69   $��348,306,170,10q�6M    �� CornerRadius$ 8��6e   ���	#FFE2E2E2._�5      �� .+�5      �� $��Confirm��6   $��27,4,-28,-2q�6*   $��14(�6?   5      5      �� 5   
   -   5        $ 	cancelBtn��5      
���   �$��1q�68   $��138,306,379,10q�6L   $ 8��6d   ���	#FFE2E2E2._�5      �� .+�5      �� $��Cancel��6   $��27,4,-28,-2q�6)   $��14(�6>   5      5      5      5      