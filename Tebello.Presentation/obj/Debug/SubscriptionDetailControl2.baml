   M S B A M L   `   `   `  ????D  @Subs.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null8*clr-namespace:Subs.Data;assembly=Subs.Data	Subs.Data  5      O KTebello.Presentation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Subs.PresentationSubs.Presentation 5      ;"clr-namespace:Tebello.PresentationTebello.Presentation 6<   2   ,Subs.Presentation.SubscriptionDetailControl2   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ;Data*clr-namespace:Subs.Data;assembly=Subs.Data     5      1localclr-namespace:Subs.Presentation     5      5local1"clr-namespace:Tebello.Presentation     6<   -   5          >?Name$  UserControl??5	   I   $
??auto??5      $
??auto??6#   $
??Left=?61   
????  ?$??3q?5	   5    ? 	Resources 5
      (  "Tebello.Presentation.DateConverter   lDateConverter(  "Tebello.Presentation.RandConverter  lRandConverter  
AlignRight ?? 
TargetType ?? Property ?? Value??%?   &	        &	      &	 &     '?? v       5   
   6     5   
   6   ?? 5   
   " ?6#   .?5      ??  ? ??$ Right=?64   6D   5      ?? 5   
   " ??6   .?5      ??  ? ??
 ?????6+   5      5      .??5      ? $??0,0,0,0q?6   $
??Auto??6   $
??Auto??6*   $??Stretch=?67   $??Stretch=?6U    ? RowDefinitions 5   
   ?? 5      $??* ?6   .+?5   
   &   System.Windows.Controls.DataGrid  -   5          Name$  subscriptionDetailDataGrid??5         AutoGenerateColumns .     EnableRowVirtualization . $
??auto??6r   $??Stretch=?6?   $??10,10,10,10q?5      	   RowDetailsVisibilityMode$	 VisibleWhenSelected=?6   
   SelectionMode$
 Single=?6K      SelectionUnit$ CellOrRowHeader=?6b      AlternatingRowBackground# ? ??5         CanUserAddRows .     CanUserDeleteRows .     FrozenColumnCount$ 5??6?      
IsReadOnly .    	RowHeight$ 30Q?5    E   $ ?? IsSynchronizedWithCurrentItem$
 TrueY?6T      RowHeaderStyle 5!      ?? 5"      :  4System.Windows.Controls.Primitives.DataGridRowHeader"  6   .?5#      ??  ? ??
 ?????64   ?? 5$       ? ??$ 30??6/   ?? 5%       ? ??
 ?   ?64   ?? 5&       ? ??$ 3q?69   5'         ColumnHeaderStyle 5*      ?? 5+      =  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6   .?5,      ??  ? ??$ 16(?61   ?? 5-       ? ??$ 3q?60   ?? 5.       ? ??
 ?????64   ?? 5/       ? ??
 ??  ?64   ?? 50       ? ??$ 3q?69   51         Columns 53      0  *System.Windows.Controls.DataGridTextColumn  54      -   5        subscriptionIdColumn??,  &System.Windows.Controls.DataGridColumn   	CellStyle# [ 54   f      Header$ Subscription Id??6?      Width5  /System.Windows.Controls.DataGridLengthConverter$
 auto 6?   1	  +System.Windows.Controls.DataGridBoundColumn 	  Binding 6C   ?? *SubscriptionId+6?     55      -   5        statusNameColumn??$ Status Name??55   m   $
 auto 6?    6?   ?? *StatusString+ ?? Mode$ OneWay=?6?     56      -   5        payerSurnameColumn??$ Payer Surname??56   b   $
 auto 6y    6A   ?? *PayerSurname+6y     57      -   5        receiverSurnameColumn??$ Receiver Surname??57   h   $
 auto 6?    6D   ?? *ReceiverSurname+6?     58      -   5        productNameColumn??$ Product Name??58   `   $
 auto 6v    6@   ?? *ProductName+6v     59      $ 	InvoiceId??6C   $
 auto 6V    6%   ?? *	InvoiceId+6V     5:      $ 
ProFormaId??6D   $
 auto 6X    6%   ?? *
ProFormaId+6X   4
  .System.Windows.Controls.DataGridCheckBoxColumn
  5;      -   5        renewColumn??$ Renewal notice??5;   `   $
 auto 6x    6>   ?? *RenewalNotice+6x   
  5<      -	   5        automaticrenewColumn??$ Automatic renew??5<   l   $
 auto 6?    6G   ?? *AutomaticRenewal+6?     5=      -
   5        payerColumn??# [ 5=   V   $ PayerId??6~   $
 auto 6?    6:   ?? *	PayerId+6?     5>      -   5        receiverColumn??# [ 5>   \   $ 
ReceiverId??6?   $
 auto 6?    6=   ?? *
ReceiverId+6?     5?      -   5        unitsPerIssueColumn??# [ 5?   d   $ Units Per Issue??6?   $
 auto 6?    6B   ?? *UnitsPerIssue+6?     5@      -   5        payerCompanyColumn??$ Payer Company??5@   b   $
 Auto 6y    6A   ?? *PayerCompany+6y   5B      5C      5D      