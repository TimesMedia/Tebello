   M S B A M L   `   `   `  ????O  KTebello.Presentation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Subs.PresentationSubs.Presentation  5   	   D @Subs.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null8*clr-namespace:Subs.Data;assembly=Subs.Data	Subs.Data 5   	   -    'Subs.Presentation.AdministrationCompany   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:Subs.Presentation     5   	   ;Data*clr-namespace:Subs.Data;assembly=Subs.Data     5   	     ? Title$  Company administration??5	   	   $??903.787??6(   $	??400??69   $
??1100??6I   $
??1226??6Y    ? WindowStartupLocation$ Manual=?5
   	    ? 	Resources 5      !  Subs.Data.AdministrationDoc   administrationDoc  companyViewSource ?? Source ?? Source ?? 
TargetType ?? Property ?? Value ?? 
TargetType	 ?? CornerRadius*  $System.Windows.Controls.DataGridCell
 ?? Triggers P? Property P? Value??%?  &	        &	      7   '?? @     '  ?      5   
   6!   ?? 5   
    69   ?? *	Company+8   ?? 5   
   " ??6   .?5      ??  ?@?OverridesDefaultStyle . 66   ?? 5       ?@?Cursor$
 Hand??6'   ?? 5       ? ??$ 20??6'   ?? 5       ? ?? 5      ?? 5      " ??6&   .R?5      ?? $??
borderZero??6!   $??2q?63   
???   ?
????ح?$	 10??6r   ._?5      ?? $??Center=?6/   $??Center=?6L   5      5      5      5      ?? 5   
   "  6   
 5      P? 5        ?@ 
IsSelected . . ?5!      ??  ? ??
 ?????63   ?? 5"       ? ??
 ??ح?63   5#      5%      5&      .??5'      ?? -   5         ??Name$ TabControl1??5'      .@?5(   
   ?? -   5        $?? Non verified companies??5(      .??5*      &   System.Windows.Controls.DataGrid  -   5          Name$ datagridCustomers??5*         CanUserDeleteRows .     SelectionMode$ Single=?6M      AutoGenerateColumns .     EnableRowVirtualization .     	RowHeight$ 30Q?5+   S   ??6b   ??    Columns 5,      0  *System.Windows.Controls.DataGridTextColumn  5-      -   5        
CustomerId??,  &System.Windows.Controls.DataGridColumn   Header$ 
CustomerId??5-   \      Width5  /System.Windows.Controls.DataGridLengthConverter$
 auto 6p      CanUserSort .    MinWidth$ 20Q?5.   8   1  +System.Windows.Controls.DataGridBoundColumn   Binding 5-   =   ?? *
CustomerId+5.   8     5/      -   5        CompanyNameUnverified??$ CompanyName unverified??5/   r   $
 auto 6?    . $ 90Q?50   8    5/   H   ?? *CompanyNameUnverified+50   8    ? ContextMenu 52      ?? 53      .@?54      m? -   5         m?Name$ contextSelect??54   #   $??Select??6:   6J   55      57      59      ?? 5:   
   $??Company template??6   .??5;      ?  ? RowDefinitions 5<      ?? 5=      $??80 ?6$   ?? 5>      $??* ?6$   ?? 5?      $??* ?6$   ?? 5@      $??* ?6$    ? ColumnDefinitions 5C      ?? 5D      $	??77* ?6'   ?? 5E      $
??226* ?6'   ?? 5F      $
??303* ?6'   ?? 5G      $
??303* ?6'   ?? 5H      $
??303* ?6'   .+?5K      ?? $??2??6   $??
20,05,0,50q?6,   .??6@   Search string6O   ?? 5L      -   5         ??Name$ SearchString??5L      $??23??60    ?? TextWrapping$
 Wrap=?6<   $	??250??6P   $??26,41,27,16q?6]   $??2??6r   ?? 5N      $??2??6   .+?5O      ?? -   5        $??buttonSearchOnCompany??5O      $??Search on company name??6:   $	??284??6\   $??23??6?   $??0,10,0,0q?6?   ?? 5P      -	   5        $#??buttonSearchOnVatregistration??5P      $??Search on VatRegistration??6B   $	??284??6f   $??24??6?   $??0,10,0,0q?6?   5Q      ?? 5R      $??3??6   .+?5S      ?? -
   5        $ ??buttonSearchOnVendorNumber??5S      $??Search on VendorNumber??6?     ?? Left$	  604??6`   
! ?? Top$! 14??6r   $	??254??6?   $??0,10,0,0q?6?   ?? 5T      -   5        $-??'buttonSearchOnCompanyRegistrationNumber??5T      $)??#Search on CompanyRegistrationNumber??6L   $	  604??6z   $! 59??6?   $	??254??6?   $??22??5U      $??0,10,0,0q?6   5V        5X      -   5        $ companyDataGrid??5X      $??1??64   $??3??6A   $??4??6R    .   . $
??Auto??6?   "   RowDetailsVisibilityMode$" VisibleWhenSelected=?5Y      $??Stretch=?6D   $??0,0,0,10q?6`   $
??Auto??5Z      $??Stretch=?6!    .  $ 30Q?6^   ??5[      ?? 5\      # ?? NotifyOnValidationError# . $ ?? UpdateSourceTrigger$$ 	LostFocus=?6A   # [ 6a   % ?? ValidationRules% 5]      5  /System.Windows.Controls.ExceptionValidationRule  5^   "   5	  /System.Windows.Controls.DataErrorValidationRule	  5_   "   ;
  5System.Windows.Controls.NotifyDataErrorValidationRule
  5`   "   5b      &   RowHeaderStyle& 5e      ?? 5f      :  4System.Windows.Controls.Primitives.DataGridRowHeader"  6    .?5g      ??  ? ??
 ?????6<   ?? 5h       ? ??$ 30??67   ?? 5i       ? ??
 ??ح?6<   ?? 5j       ? ??$ 3q?6A   5k       5m        5n      -   5        CompanyColumn??$ Company??5n   E   $
 auto 6V   $	 250Q?6c    .  5o      ?? 5p   "   ' ?? Path$' CompanyName?6*   5q   #   5t        5u      -   5        VatRegistrationColumn??$ VatRegistration??5u   p   $
 auto 6?    . $ 80Q?5v   8    5u   L   ?? *VatRegistration+5v   8     5w      -   5        VendorNumber??$ VendorNumber??5w   d   $
 auto 6z    . $ 90Q?5x   8    5w   C   ?? *VendorNumber+5x   8     5y      -   5        CompanyRegistrationNumber??$ CompanyRegistrationNumber??5y   ~   $
 auto 6?    . $ 90Q?5z   8    5y   P   ?? *CompanyRegistrationNumber+5z   8   5|      ?? 5?      $ ??Save company template data??6   $	??200??6A   $??4??6N   $??1??6^   $??	0,28,0,27q?6k   .??5?      ?? $??Bottom=?6!   .+?5?      ?? -   5        $??buttonUpdateCompany??5?   !   $??Update??6<   $	??164??6M   $??0,10,0,0q?6{   ?? 5?      -   5        $??buttonAddCompany??5?   !   $	??Add??6:   $	??164??6H   $	! 391??6T   $??25??6?   $??0,10,0,0q?6?   ?? 5?      -   5        $??buttonRemoveCompany??5?   !   $??Remove unlinked companies??6<   $	??164??6`   $??	0,10,0,10q?5?      ( ? ToolTip$2( ,Remove companies not linked to any customer.??60   $??30??6g   5?      5?      ?? 5?      $??2??6   $??4??6)   $??Consolidate companies??6;   $	??200??6Z   $	??Top=?6g   .??5?      ?? .+?5?      ?? -   5        $??buttonSelectSource??5?   !   $??Mark selection as source??6;   $	??164??6^   $??0,10,0,0q?6?   ?? 5?      -   5        $??
textSource??5?   "   $	??273??64   $??0,10,0,0q?6@   ?? 5?      -   5        $??buttonSelectTarget??5?   !   $??Mark selection as target??6;   $	??164??6_   $??0,10,0,0q?6?   ?? 5?      -   5        $??
textTarget??5?   "   $	??273??64   $??0,10,0,0q?6@   ?? 5?      -   5        $??buttonConsolidateCompany??5?   !   $??Consolidate??6A   $	??164??6X   $??0,10,0,0q?6?   5?      5?      ?? 5?      $ ??Verify company on customer??6   $	??200??6?   $??4??6L   $??3??6\   $??	0,28,0,27q?6i   .??5?      ?? -   5        $??buttonUpdateCustomer??5?      $??Verify??69   $	??164??6J   $??0,10,0,0q?6y   5?      5?      5?      5?      5?      