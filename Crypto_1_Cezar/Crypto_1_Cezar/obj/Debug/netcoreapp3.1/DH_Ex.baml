   M S B A M L   `   `   `  ����I  ECrypto_1_Cezar, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Crypto_1_CezarCrypto_1_Cezar  5   	       Crypto_1_Cezar.DH_Ex   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:Crypto_1_Cezar     5   	     � Title$  DH_Ex��5   	   $	��550��6   $	��800��6$    � 	Resources 5	       �� Color   	TextColor  FieldBlackColor  MainBlackColor �� 
TargetType �� Property �� Value��%*  &	        &	 &     &	 L     '�� r     7   �� 5
   
   $ #e6e6e6��6   6*   �� 5   
   $ #1e1e1e��6   6*   �� 5   
   $ #2d2d30��6   6*   �� 5   
   " ��6   .�5      ��  � ��$ 33(�6)   �� 5       � ��$ 15q�6'   �� 5       � ��8   6+   5      5      .��5      � #��[ 6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��Right=�6"   .+�5      ��  �� Orientation$ 
Horizontal=�6   .+�5      �� $��P��6   $��Right=�6%   �� 5       -   5        	 ��Name$	 PField��5       $�� ��6,   $��Right=�64   5!      �� 5"      $��Alise��6   $��Right=�6%   �� 5$      $ 
Horizontal=�6   .+�5%      �� $��
Secrey Key��6   $��Right=�6.   �� 5&      -   5        $	 ASKey��5&      $�� ��6+   $��Right=�5'      
���  ��5(      5)      �� 5+      $ 
Horizontal=�6   .+�5,      �� $��
Middle Key��6   $��Right=�6.   �� 5-      -   5        $	 AMKey��5-      $�� ��6+   $��Right=�63   5.      �� 5/      $ 
Horizontal=�6   .+�50      �� $��	Final Key��6   $��Right=�6-   �� 51      -   5        $	 AFKey��51      $�� ��6+   $��Right=�63   52      54      �� 55   
   $��0��6   $��1��6"   $
��Left=�62   .+�56      �� $ 
Horizontal=�6   .+�57      �� $��G��6   $��Right=�6%   �� 58      -   5        $	 GField��58      $�� ��6,   $
��Left=�64   59      �� 5:      $	��Bob��6   $
��Left=�6#   �� 5<      $ 
Horizontal=�6   .+�5=      �� $��
Secret Key��6   $
��Left=�6.   �� 5>      -   5        $	 BSKey��5>      $�� ��6+   $
��Left=�63   
���  ��6N   5?      �� 5A      $ 
Horizontal=�6   .+�5B      �� $��
Middle Key��6   $
��Left=�6.   �� 5C      -   5        $	 BMKey��5C      $�� ��6+   $
��Left=�63   5D      �� 5E      $ 
Horizontal=�6   .+�5F      �� $��	Final Key��6   $
��Left=�6-   �� 5G      -   5        $	 BFKey��5G      $�� ��6+   $
��Left=�63   5H      5I      �� 5K   
   -	   5        
 ��Name$
 generateButt��5N      $��2��5L      $��1��5M      $��70��5O      $	��170��5P      $��35(�5Q      $��Generate��5R      6    5S      5T      