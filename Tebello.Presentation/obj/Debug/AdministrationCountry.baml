   M S B A M L   `   `   `  ????O  KTebello.Presentation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Subs.PresentationSubs.Presentation  5   	   D @Subs.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null8*clr-namespace:Subs.Data;assembly=Subs.Data	Subs.Data 5   	   ;"clr-namespace:Tebello.PresentationTebello.Presentation  6A   -    'Subs.Presentation.AdministrationCountry   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:Subs.Presentation     5   	   ;Data*clr-namespace:Subs.Data;assembly=Subs.Data     5   	   5local1"clr-namespace:Tebello.Presentation     6A   -   5          ? Title$  Administration of country??5	   	   $
??1000??6B    ? 	Resources 5
      "  Subs.Data.DeliveryAddressDoc   deliveryAddressDoc  countryViewSource ?? Source ?? Source ! countryDeliveryCostViewSource ?? 
TargetType ?? Property ?? Value  
AlignRight(   "Tebello.Presentation.RandConverter  lRandConverter??%?  &	        &	      7   &	 @     7  '?? }     &	 ?     &	 6      5   
   6"   ?? 5   
    69   ?? *	Country+8   ?? 5   
    6E   ?? *FK_DeliveryCost_Country+8   ?? 5   
   " ??6   .?5      ??  ? ??
 ????6+   ?? 5       ? ??$ 22??6'   5      ?? 5   
   " ?6#   .?5      ??  ? ??$ Right=?64   5        5   
   6   5      .??5      ??  ? DataContext# [ 6   $
??auto??6B   $??Stretch=?6O   $??0,0,0,0q?6m   .@?5   
   ?? $??Country??6   .??5      ?  ? ColumnDefinitions 5      ?? 5      $
??auto ?6'   ?? 5      $	??100 ?6'   	 ? RowDefinitions	 5      ?? 5      6&   .+?5!      &   System.Windows.Controls.DataGrid  -   5        
  Name$
 countryDataGrid1??5!         AutoGenerateColumns .     EnableRowVirtualization . $
??Auto??6p   $??Stretch=?6~      RowDetailsVisibilityMode$ VisibleWhenSelected=?5"   )   $
??Auto??6X   ??5#   !   ?? # [    Columns 5$      0  *System.Windows.Controls.DataGridTextColumn  5%      -   5        countryNameColumn1??,  &System.Windows.Controls.DataGridColumn   Header$ Country Name??5%   i      Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6I   ?? *CountryName+6   5'      ?? 5(      -   5         ??Name$ buttonCountry??5(      $??Submit??60   $??1??6A   $??75??6Q   $??Bottom=?6x   5)      5*      ?? 5,   
   $??DeliveryCost??6   .??5-      ? ???	#FFE5E5E5# [ 6*   	 5.      ?? 5/      $??* ?6$   ?? 50      $??40 ?6$    52      ?? 53      $
??auto ?6'   ?? 54      $
??auto ?6'   .+?57        -   5        $
 countryDataGrid??57       .   . $ Visible=?58      $
??Auto??6A   $??Stretch=?6N   $
??Auto??6m   $??Stretch=?6{   $??2??59      ??57   o   ??  5:        5;      -   5        countryNameColumn??$ Country Name??5;   h   $
 Auto 6~    6H   ?? *CountryName+6~   5=        5?      -   5        $%
 fK_DeliveryCost_CountryDataGrid??5?      $??1??6D    .   . $
??Auto??6?      SelectionUnit$ FullRow=?5@      $	??Top=?64   $ Visible=?5A   k   $??10,0,0,0q?5B      ??5A      ?? # [  5C      4  .System.Windows.Controls.DataGridTemplateColumn  5D      -   5        dateFromColumn??$ 	Date From??5D   I   $	 100 6\      CellTemplate 5E      ?? 5F   "   .R?5G   &   (	  "System.Windows.Controls.DatePicker	   	  SelectedDate 61   ?? *
DateFrom+ ?? Mode$ TwoWay=? ?? NotifyOnValidationError .  ?? ValidatesOnExceptions . 5H   #   5J        5K      -	   5        mail1Column??   	CellStyle# [ 5K   ?   $ Mail less than 1??6?   $ SizeToHeader 6?    6B   ?? *Mail1+ ?? 	Converter# [ 6?     5L      -
   5        mail2Column??$ Mail less than 5??5L   ?   $ SizeToHeader 6?   # [ 6?    6B   ?? *Mail2+# [ 6?     5M      -   5        mail3Column??$ Mail more than 5??5M   ?   $ SizeToHeader 6?   # [ 6?    6B   ?? *Mail3+# [ 6?     5O      -   5        courier1Column??$ Courier less than 1??5O   ?   $ SizeToHeader 6?   # [ 6?    6E   ?? *
Courier1+# [ 6?     5P      -   5        courier2Column??$ Courier less than 5??5P   ?   $ SizeToHeader 6?   # [ 6?    6E   ?? *
Courier2+# [ 6?     5Q      -   5        courier3Column??$ Courier more than 5??5Q   ?   $ SizeToHeader 6?   # [ 6?    6E   ?? *
Courier3+# [ 6?   5S      ?? 5T      -   5        $??buttonSubmitDeliveryCost??5T      $??Submit??69   $??1??6J   $??1??6W   $	??106??6g   $
??Left=?6s   $??	10,10,0,0q?5U   @   5V      5W      5X      5Y      