   M S B A M L   `   `   `  ����D  @Personate, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:Personate.MVVM.ViewPersonate.MVVM.View  5   	   %clr-namespace:Personate	Personate  5   	   C&clr-namespace:Personate.MVVM.ViewModelPersonate.MVVM.ViewModel  5   	   $    Personate.MVVM.View.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:Personate.MVVM.View     5   	   *globalclr-namespace:Personate     5   	   <	viewmodel&clr-namespace:Personate.MVVM.ViewModel     5   	     � 
ResizeMode$  NoResize=�5	       � Title$ 	Personate��5
   	   $	��540��6   $	��960��6(    � WindowStyle$
 None=�5   	   
���   � � WindowStartupLocation$ CenterScreen=�5   	    � DataContext 5      ,  &Personate.MVVM.ViewModel.MainViewModel  5   
   .��5      �  � ColumnDefinitions 5   
   �� 5      $	��150 �6   �� 5       � RowDefinitions 5   
   �� 5      $��80 �6   �� 5      .+�5   
   �� $��Center=�6   $��Center=�5      .+�5      �� .+�5      �� $��	Personate��6   $��24(�5         DarkText#���   5       �� 5!      $��Egxr41k software��6   $
��13.5(�5"       J� Opacity$	 0.6Q�6,   #���   5#      5$      5%      �� 5'   
   -   5         ��Name$
 Menu��5'   1    
 DarkBg#��[ 6   $��1��6?   ._�5(      �� $��Center=�6   .+�5*      �� $
��Home��6     DarkMenuBtn#���  5+      ��5,      �� *HomeViewCommand+�� 5.      $��Fonts��6   #���  5/      ��50      �� *FontsViewCommand+�� 52      $��Themes��6   #���  53      ��54      �� *ThemesViewCommand+�� 56      $��Icons��6   #���  57      ��58      �� *IconsViewCommand+�� 5:      $��
Wallpapers��6   #���  5;      ��5<      �� *WallsMenuViewCommand+�� 5>      $��Taskbar��6   #���  5?      ��5@      �� *TaskbarViewCommand+�� 5B      $��Cursors��6   #���  5C      ��5D      �� *CursorsMenuViewCommand+�� 5F      $��Settings��6   #���  5G      ��5H      �� *SettingsViewCommand+5J      5K      �� 5M   
   $��1��6   	 �� Orientation$	 
Horizontal=�5N      $	��Top=�5O      $��Right=�6-   .+�5P      �� 
������ $	��700��62   �� 5Q      $	 
Horizontal=�6   .+�5R      �� $��30��6   $��30��6)   .+�5S      �� $��25��5T      $��25��6(   $	��2.5q�5U        
IconsTheme#���  5V      ��5S      �� *MinimizeIcon+
 J� InputBindings
 5W      d� 5X       d� Gesture$ 	LeftClicka�6O    �� Command 6+   �� *MinimizeCommand+6O   5Z      5[      �� 5\      $��30��6   $��30��6)   .+�5]      �� $��25��5^      $��25��6(   $	��2.5q�63   #���  5_      ��5]      �� *MaximizeIcon+
 5`      d� 5a      $ 	LeftClicka�6O    6+   �� *MaximizeCommand+6O   5c      5d      �� 5e      $��30��6   $��30��6)   .+�5f      �� $��25��5g      $��25��6(   $	��2.5q�63   #���  5h      ��5f      �� *	CloseIcon+
 5i      d� 5j      $ 	LeftClicka�6L    6+   �� *CloseCommand+6L   5l      5m      5n       � 5o      -   5        $��Mainsection��5o      $��1��6'   $��3��67   $��0, 30, 0, 0q�6H   5p      �� 5r   
   $��1��6   $��2��5s      $��0, 40, 0, 10q�5t      ��5u      �� *CurrentView+5w      5x      