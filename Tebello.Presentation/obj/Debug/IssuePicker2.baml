   M S B A M L   `   `   `  ????O  KTebello.Presentation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Subs.PresentationSubs.Presentation  5   	   D @Subs.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null8*clr-namespace:Subs.Data;assembly=Subs.Data	Subs.Data 5   	   ;"clr-namespace:Tebello.PresentationTebello.Presentation  6A   $    Subs.Presentation.IssuePicker2   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:Subs.Presentation     5   	   ;Data*clr-namespace:Subs.Data;assembly=Subs.Data     5   	   5local1"clr-namespace:Tebello.Presentation     6A   -   5          ? Title$  IssuePicker??5	   	   $
??1070??64   $
??1070??6D    ? 	Resources 5
      *  $System.Windows.Controls.DataGridCell ?? 
TargetType ?? Triggers P? Property P? Value ?? Property ?? Value:  4System.Windows.Controls.Primitives.DataGridRowHeader   
AlignRight(   "Tebello.Presentation.DateConverter  lDateConverter??%?  '        '  ?     &	  e    &	 ?    ?? 5   
   "  6    5      P? 5       ?@ 
IsSelected . . ?5      ??  ? ??
 ? ???63   ?? 5       ? ??
 ?  ??63   5      5      ?? 5   
   "  6   .?5      ??  ? ??
 ? ???6,   ?? 5       ? ??$ 30??6'   ?? 5       ? ??
 ?  ??6,   ?? 5       ? ??$ 3q?61   5      ?? 5   
   " ?6#   .?5      ??  ? ??$ Right=?64   5        5   
   6   5      .??5!      ?  ? ColumnDefinitions 5"   
   ?? 5#      $
??auto ?6   ?? 5$      $??* ?6   	 ? RowDefinitions	 5(   
   ?? 5)      $??30 ?6   ?? 5*      $??* ?6   .+?5-   
   &   System.Windows.Controls.DataGrid  -   5        
  Name$
 ProductDataGrid??5-         RowDetailsVisibilityMode$ VisibleWhenSelected=?6,      EnableRowVirtualization .    AutoGenerateColumns .     	RowHeight$ 30Q?5.         
IsReadOnly . $??2??64   $??5,5,4,5q?6E      SelectionMode$ Single=?6V   $ ?? IsSynchronizedWithCurrentItem$
 TrueY?6m      Columns 52      0  *System.Windows.Controls.DataGridTextColumn  53      ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6%      Header$ Status??62      CanUserSort .     
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6B   ?? *Status+ ?? Mode$ OneTime=?6     54      -   5        productIdColumn??$ SizeToHeader 54   >   $ 
Product Id??6S      	CellStyle# [  6?    6g   ?? *	ProductId+6?   4	  .System.Windows.Controls.DataGridTemplateColumn	  56      $ Product Name??6)   $	 300 6?    .  	  CellTemplate 57      ?? 58      .R?59      ?? $
??Auto??6&   $??3q?64    ?? TextWrapping$
 Wrap=?6?   .??6S   ??5:   "   ?? 5;   &    ?? Path$ ProductName?6.    ?? NotifyOnValidationError .  ?? ValidationRules 5<   *   5
  /System.Windows.Controls.ExceptionValidationRule
  5=   .   5?   '   .??5@   0   5A      5B      5D      5G      ?? 5I   
   -   5         ??Name$ CheckAllIssues??5I      $??1??6+   $??125,5,121,5q?6;     d? IsThreeState  .  ! d? 	IsChecked$! FalseY?6?   $??Center=?6?   .??6?   Show all issues6?     5K   
   -   5        $
 IssueDataGrid??5K      $??1??6*   $??1??6;    .   . $ 30Q?5L      $ VisibleWhenSelected=?6"   $
??Auto??6Q    . $??6,5,3,5q?6p   $
 TrueY?5M   ?    5N        5O      -   5        issueIdColumn??$ Issue Id??5O   X    . $
 Auto 6|   # [  6?    6<   ?? *	IssueId+6?     5P      -   5        issueDescriptionColumn??$ Issue Description??5P   j   $
 Auto 6?    6E   ?? *IssueDescription+6?     5Q      -   5        
yearColumn??$
 Year??5Q   R   $
 Auto 6`    69   ?? *Year+6`     5R      -	   5        noColumn??$ No??5R   N   $ SizeToHeader 6Z   # [  6o    67   ?? *No+6o     5S      $ 
Start Date??6o   $
 Auto 6?    6%   ?? *	StartDate+" ?? 	Converter#" [ 6?     5T      $ End Date??6m   $
 Auto 6    6%   ?? *	EndDate+#" [ 6   5V      5X      5Y      