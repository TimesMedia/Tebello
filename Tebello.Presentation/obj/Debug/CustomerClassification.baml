   M S B A M L   `   `   `  ????O  KTebello.Presentation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Subs.PresentationSubs.Presentation  5   	   .    (Subs.Presentation.CustomerClassification   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:Subs.Presentation     5   	   -   5          ?Name$  
MainWindow??5       ? Title$ CustomerClassification??5   	   $	??450??6(   $	??800??65    ? 	Resources 5
       ?? 
TargetType   ButtonTemplate ?? CornerRadius ?? RadiusX ?? RadiusY ?? 
TargetType ?? Property	 ?? Value??%j  &	        '?? ?     7   ?? 5   
   " ??6   .R?5      ?? $ 8??6   
???????$??2q?6:   ._?5      ? .+?5      ?? 
????ح?$ 10??61   $ 20??6>   ?? 5      $??Center=?6'   $??Center=?6D   6a   5      5      5      ?? 5   
   " ??6   .?5      ??  ? ??$		 120??6&   ?? 5       ? ??8	   6)   5      5      .??5      ?? .@?5   
   ?? $??By customer??6   .??5      ?? $??0,10,0,0q?6   .+?5      ? $??	10,10,0,0q?6   
 ? ColumnDefinitions
 5      ?? 5       ?? 5!       ? RowDefinitions 5#      ?? 5$      ?? 5%      ?? 5&      .+?5(      ?? $??0??6   $??CustomerId:??6)   ?? 5)      $??1??6   $??Surname:??6)   ?? 5*      $??2??6   $??Company:??6)   ?? 5+      -   5         ??Name$ 
CustomerId??5+   9   $??0??6   $??1??6)   69   ?? 5,      -   5        $ Surname??5,   9   $??1??6   $??1??6)   69   ?? 5-      -   5        $ Company??5-   9   $??2??6   $??1??6)   69   5.      &   System.Windows.Controls.DataGrid  50      -   5          Name$ Classifications??50      $
??Auto??64   $
??Auto??6A      	RowHeight$ 30Q?6O      SelectionMode$ Single=?6^      AutoGenerateColumns .  $??	10,10,0,0q?6?   $ ?? IsSynchronizedWithCurrentItem$
 TrueY?51         
IsReadOnly .    RowHeaderStyle 52      ?? 53      :  4System.Windows.Controls.Primitives.DataGridRowHeader"  6    .?54      ??  ? ??
	 ? ???6<   ?? 55       ? ??$	 30??67   ?? 56       ? ??
	 ?  ??6<   ?? 57       ? ??$	 3q?6A   58         Columns 5:      0  *System.Windows.Controls.DataGridTextColumn  5;      -   5        Level1IdColumn??,  &System.Windows.Controls.DataGridColumn   Header$ Level1??5;   `      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6E   ?? *Level1+6p     5<      -   5        Level2IdColumn??$ Level2??5<   `    .  6E   ?? *Level2+6p     5=      -   5        Level3IdColumn??$ Level3??5=   `    .  6E   ?? *Level3+6p    ? ContextMenu 5?      ?? 5@      .@?5A      m? -	   5         m?Name$ MenuItemRemove??5A   '   $??Remove??6?   6o   5B      5D      ?? 5E      -
   5         ??Name$ ButtonAddClassification??5E      $??Add a classification??6:   $	??200??6Y   $
??Left=?6e   $??	10,10,0,0q?6?   6?   ?? 5F      -   5        $ ButtonSingleTest??5F      $)??#Test integrity for current customer??63   $	??200??6a   $
??Left=?6m   $??	10,10,0,0q?6?   6?   ?? 5G      -   5        $ ButtonAllTest??5G      $y?Hidden=?60   $&?? Test integrity for all customers??6E   $	??180??6p   $
??Left=?6|   $??	10,10,0,0q?6?   6?   5I      5J      ?? 5L   
   $??By classification??6   $y?Hidden=?6-   .??5M      ?? .+?5N      ?? -   5        $ 
buttonPick??5N      $??Pick a Classification??6)    ?? Left$ 27??6I   
 ?? Top$ 24??6Z   $	??105??6j   6v   5O      5P      5Q      5R      