   M S B A M L   `   `   `  ????O  KTebello.Presentation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:Tebello.PresentationTebello.Presentation  5   	   D @Subs.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null8*clr-namespace:Subs.Data;assembly=Subs.Data	Subs.Data 5   	   2    ,Tebello.Presentation.AdministrationPromotion   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:Tebello.Presentation     5   	   ;Data*clr-namespace:Subs.Data;assembly=Subs.Data     5   	   $
??1000??5        ? Title$  AdministrationPromotion??5	   	   $
??auto??6)    ? 	Resources 5
         promotionViewSource  
AlignRight ?? 
TargetType ?? Property ?? Value(   "Tebello.Presentation.RandConverter  lRandConverter??%?   &	        &	      &	 ^     ?? 5   
   6   ?? 5   
   " ?6#   .?5      ??  ? ??$ Right=?64   5        5   
   6   5      .??5      ?? .@?5   
   ?? $??	Promotion??6   .??5      ?  ? ColumnDefinitions 5      ?? 5      $	??56* ?6'   ?? 5      $
??321* ?6'    ? RowDefinitions 5      ?? 5      $??* ?6$   ?? 5      $??60 ?6$   .+?5      &   System.Windows.Controls.DataGrid  -   5          Name$ PromotionDataGrid??5       ? DataContext# [  66   	   AutoGenerateColumns	 .  
   EnableRowVirtualization
 .    RowDetailsVisibilityMode$ VisibleWhenSelected=?5   7   $
??auto??6f      CanUserDeleteRows .    CanUserAddRows .     SelectionUnit$ FullRow=?5   O      SelectionMode$ Extended=?5       $
??auto??69   $??Stretch=?6G   $ ?? IsSynchronizedWithCurrentItem$
 TrueY?6c   $??2??5!   B   ??5      ??    RowHeaderStyle 5"      ?? 5#      :  4System.Windows.Controls.Primitives.DataGridRowHeader"  6    .?5$      ??  ? ??
 ? ???6<   ?? 5%       ? ??$ 30??67   ?? 5&       ? ??
 ?  ??6<   ?? 5'       ? ??$ 3q?6A   5(       ? ContextMenu 5+      ?? 5,      .@?5-      m? -   5         m?Name$ MenuItemPayer??5-   '   $??Insert a payer??6?   6Y   m? 5.      -   5        $ MenuItemProduct??5.   '   $??Insert a product??6@   6[   m? 5/      -   5        $ MenuItemPayerClear??5/   '   $??Clear a payer??6C   6[   50         Columns 53      0  *System.Windows.Controls.DataGridTextColumn  55      -   5        PayerColumn??,  &System.Windows.Controls.DataGridColumn   Header$ Payer??55   p      Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6B   ?? *PayerSurname+ ?? Mode$ OneWay=?6?     57      -   5        ProductColumn??$ Product??57   q   $
 auto 6?    .  6D   ?? *ProductName+$ OneWay=?6?     59      -   5        DiscountPercentageColumn??$ Percentage discount??59   ?      	CellStyle# [ 5:   1   $
 Auto 6Z    59   O   ?? *DiscountPercentage+ ?? 	Converter# [ 5:   Z   4  .System.Windows.Controls.DataGridTemplateColumn  5<      -   5        StartDateColumn??$ 
Start date??5<   J   $
 auto 6^      CanUserSort .    SortMemberPath$ 	StartDate??6~      CellTemplate 5=      ?? 5>   "   .R?5?   &   (	  "System.Windows.Controls.DatePicker	   	  SelectedDate 61   ?? *	StartDate+$ TwoWay=?  ?? NotifyOnValidationError  . ! ?? ValidatesOnExceptions! . " ?? UpdateSourceTrigger$" PropertyChanged=?5@   #   5B        5D      -	   5        EndDateColumn??$ End date??5D   H   $
 auto 6Z    . $ EndDate??6z    5E      ?? 5F   "   .R?5G   &   	   5H   *   ?? 5I   .   # ?? Path$# EndDate?66   $ TwoWay=?6E     . ! . $" PropertyChanged=?6?   $ ?? ValidationRules$ 5J   2   5
  /System.Windows.Controls.ExceptionValidationRule
  5K   6   5M   /   5O   '   5P   #   5R      5T      ?? 5V      % ?? Orientation$% 
Horizontal=?6   $??1??66   $??10q?6C   $??2??6O   .+?5W      ?? -
   5        & ??Name$& 	buttonAdd??5W      $??Add a promotion??60   $??	0,10,10,0q?6J   $	??145??6]   6i   ?? 5X      -   5        $& buttonSubmit??5X      $??Submit??63   $??	0,10,10,0q?6D   $	??145??6W   6c   ?? 5Y      -   5        $& 
buttonExit??5Y      $??Exit without saving??61   $??	0,10,10,0q?6O   $	??145??6b   6n   5Z      5[      5\      ?? 5]   
   $??Bulletin board??6   .??5^      ?? .+?5_      ?? -   5        ' ??Name$' BulletinEntry??5_      $	??407??61   $	??400??6?   $??227,0q?6K   ( ?? AcceptsReturn( . 6q   ?? 5`      -   5        $& SaveBulletinEntry??5`      $??80??6T   $??0,10,0,0q?6_   .??6q   Save6w   5a      5b      5c      5d      