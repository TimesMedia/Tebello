   M S B A M L   `   `   `  ????O  KTebello.Presentation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Subs.PresentationSubs.Presentation  5      ;"clr-namespace:Tebello.PresentationTebello.Presentation  6<   9    3Subs.Presentation.SubscriptionStatusDisplayControl2   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:Subs.Presentation     5      5local1"clr-namespace:Tebello.Presentation     6<   $	??800??5        ? 	Resources  5	      (   "Tebello.Presentation.RandConverter   lRandConverter+   %Tebello.Presentation.IntegerConverter  lIntegerConverter(   "Tebello.Presentation.DateConverter  lDateConverter  
AlignRight ?? 
TargetType ?? Property ?? Value??%?   &	        &	      &	 &     &	 9       5
   
   6     5   
   6"     5   
   6   ?? 5   
   " ?6#   .?5      ??  ? ??$ Right=?64   5      5      .??5      ?? .??5   
   ?? .+?5      ? -   5         ?Name$ Basic??5      $??10,0,0,0q?6"    ? ColumnDefinitions 5      ?? 5      $	??189 ?6'   ?? 5      $	??189 ?6'   ?? 5      $	??189 ?6'   ?? 5      $	??189 ?6'    ? RowDefinitions 5      ?? 5      ?? 5      .+?5!      ?? $??0??6   $??4??6)   $??0,0,0,20q?6=   $??20(?6P   $
??Bold"?5"      
???  ??!Subscription status and history6a   ?? 5#      $??1??6   $??0??6%   $??Subscription id:??65   ?? 5$      $??1??6   $??1??6&    ? UseLayoutRounding . 
????  ?$	??150??5%   +   ??5$   q   ?? *SubscriptionId+5%   +   5&      ?? 5(      $??	10,20,0,0q?6   ??5)      ?? 5*      $??	0,10,0,10q?6    
???  ???? 5+   &   .?6+   Basic information6>   5,      .??50      ?  .  ? DataContext 6   ?? 	 ?? ElementName$	 Basic??
 ?? Path$
 DataContext? 51      ?? 52      $	??150 ?6+   ?? 53      $	??200 ?6+   ?? 54      $	??150 ?6+   ?? 55      $	??150 ?6+    57      ?? 58      ?? 59      ?? 5:      ?? 5;      ?? 5<      ?? 5=      ?? 5>      ?? 5?      .+?5B      ?? $??PayerId??6    . 6.   ?? 5C      $??1??68   
????  ? . ??6   ?? *	PayerId+6Z   ?? 5D      $??2??6   
????  ? . .??5E      ?? $??WrapWithOverflow=?6E   ??6$   ?? *ReceiverSurname+6E   5F      ?? 5G      $??3??6   
????  ? . .??5H      ?? $??WrapWithOverflow=?6B   ??6$   ?? *PayerCompany+6B   5I      ?? 5K      $??1??6   $??Receiver??6)    . 6<   ?? 5L      $??1??6   $??1??6H   
????  ? . ??6)   ?? *
ReceiverId+6j   ?? 5N      $??1??6   $??2??6)   
????  ? . .??5O      ?? $??WrapWithOverflow=?6E   ??6$   ?? *ReceiverSurname+6E   5P      ?? 5Q      $??1??6   $??3??6)   
????  ? . .??5R      ?? $??WrapWithOverflow=?6E   ??6$   ?? *ReceiverCompany+6E   5S      ?? 5U      $??2??6   $??Status??6)    . 6:   ?? 5V      $??2??6   $??1??6J   
????  ? . ??6)   ?? *StatusString+6l   ?? 5Y      $??3??6   $??DeliveryMethod??6)    . 6B   ?? 5Z      $??3??6   $??1??6R   
????  ? . ??6)   ?? *DeliveryMethodString+6t   ?? 5]      $??4??6   $??Product Name??6)    . 6@   ?? 5^      $??4??6   $??1??6)   
????  ? . .??5_      ?? $??WrapWithOverflow=?6A   ??6$   ?? *ProductName+6A   5`      ?? 5b      $??5??6   $??Number of issues??6)    . 6D   ?? 5c      $??5??6   #??[ 6z   $??1??5d      
????  ? . ??5c   )   ?? *NumberOfIssues+ ?? 	Converter# [ 5d   ?   ?? 5f      $??6??6   $??Copies per issue??6)    . 6D   ?? 5g      $??6??6   #??[ 6y   $??1??5h      
????  ? . ??5g   )   ?? *UnitsPerIssue+# [ 5h   ?   ?? 5j      $??7??6   $??Vat percentage??6)    . 6B   ?? 5k      $??7??6   #??[ 6x   $??1??5l      
????  ? . ??5k   )   ?? *VatPercentage+# [  5l   ?   ?? 5n      $??3??6   $??2??6)   $??Renewal Notice??69    . 6R   ?? 5o      $??3??6   $??3??6)   
????  ? . ??69   ?? *RenewalNotice+6n   ?? 5r      $??4??6   $??2??6)   $??Automatic renewal??69    . 6U   ?? 5s      $??4??6   $??3??6)   
????  ? . ??69   ?? *AutomaticRenewal+6q   ?? 5v      $??5??6   $??2??6)   $??DeliveryAddressId??69    . 6U   ?? 5w      $??5??6   $??3??6)   
????  ? . ??69   ?? *DeliveryAddressId+6r   ?? 5?      $??6??6   $??2??6)   $??Order number??69    . 6P   ?? 5?      $??6??6   $??3??6)   
????  ? . ??69   ?? *OrderNumber+6l   ?? 5?      $??7??6   $??2??6)   $??	InvoiceId??69    . 6M   ?? 5?      $??7??6   $??3??6)   
????  ? . ??69   ?? *	InvoiceId+6j   5?      5?      ?? 5?      $??	10,20,0,0q?6   ??5?      ?? 5?      
???  ???? 5?   &   .?6+   Calculation68   5?      .??5?      ?  .  6   ?? $	 Basic??$
 DataContext? 5?      ?? 5?      $??41.435 ?6+   ?? 5?      $??158.565 ?6+   ?? 5?      $	??100 ?6+    5?      ?? 5?      ?? 5?      ?? 5?      ?? 5?      ?? 5?      ?? 5?      ?? 5?      ?? 5?      ?? 5?      ?? 5?      ?? 5?      ?? 5?      .+?5?      ?? $??Base cost per unit??6   $??0??69    . $??2??6?   ?? 5?      $??2??6   
????  ?$??0??6>   #??[ 6K   ??5?      ?? $
 BaseRate?# [  ?? 5?      $??Discount per unit??6   $??1??68    . $??2??6   ?? 5?      $??2??6   
????  ?$??1??6>   #??[ 6K   ??5?      ?? $
 DiscountPerUnit?# [  ?? 5?      $??2??6   $??2??6(   
????  ?	 ?? X1$ 0??6F   	 ?? Y1$ 0??6M   	 ?? X2$	 100??6T   	 ?? Y2$ 0??6]   $??0,0,0,28q?6d   $??2??6v   6?   ?? 5?      $!??Adjusted base rate per unit??6   $??3??6B    . $??2??6?   ?? 5?      $??2??6   
????  ?$??3??6>   #??[ 6K   ??5?      ?? $
 AdjustedBaseRate?# [  ?? 5?      $??Delivery cost per unit??6   $??4??6=    . $??2??6?   ?? 5?      $??2??6   
????  ?$??4??6>   #??[ 6K   ??5?      ?? $
 DeliveryCost?# [  ?? 5?      $??Vat per unit??6   $??5??63    . $??2??6z   ?? 5?      $??2??6   $??5??6,   
????  ?#??[ 6K   ??5?      ?? $	
 Vat?# [  ?? 5?      $??6??6   $??2??6(   
????  ?$ 0??6F   $ 0??6M   $	 100??6T   $ 0??6]   $??0,0,0,28q?6d   $??2??6v   6?   ?? 5?      $??	Unit cost??6   $??7??60    . $??2??6w   ?? 5?      $??2??6   $??7??6,   
????  ?#??[ 6K   ??5?      ?? $
 	UnitPrice?# [  ?? 5?      $??Number of units??6   $??8??66    . $??2??6}   ?? 5?      $??2??6   
????  ?$??8??6>   #??[ 6K   ??5?      ?? $
 NumberOfUnits?# [ ?? 5?      $??9??6   $??2??6(   
????  ?$ 0??6F   $ 0??6M   $	 100??6T   $ 0??6]   $??0,0,0,28q?6d   $??2??6v   6?   ?? 5?      $??
Total cost??6   $??10??61    . $??2??6y   ?? 5?      $??2??6   
????  ?$??10??6>   #??[ 6L   ??5?      ?? $
 	TotalCost?# [  ?? 5?      $??11??6   $??2??6)   
????  ?$ 0??6G   $ 0??6N   $	 100??6U   $ 0??6^   6g   5?      5?      ?? 5?      $??	10,20,0,0q?6   ??5?      ?? 5?      
???  ??$??	0,10,0,10q?61   ?? 5?   &   .?6+   Issue information6>   5?      .??5?      &   System.Windows.Controls.DataGrid  -   5          Name$ Issues??5?         AutoGenerateColumns .     EnableRowVirtualization . $
??Auto??6g      
IsReadOnly .    RowDetailsVisibilityMode$ VisibleWhenSelected=?5?   /   $??Stretch=?5?      ??5?   u   ??    RowHeaderStyle 5?      ?? 5?      6"      Columns 5?      4  .System.Windows.Controls.DataGridTemplateColumn  5?      ,  &System.Windows.Controls.DataGridColumn   Header$ Issue Description??61      Width5  /System.Windows.Controls.DataGridLengthConverter$	 300 6L      CellTemplate 5?      ?? 5?   "   .R?5?   &   ?? $
??Auto??6.   $??3q?6<    ?? TextWrapping$
 Wrap=?6G   
????  ?.??6m   ??5?   *   ?? 5?   .   $
 IssueDescription?66   5?   /   .??5?   8   5?   '   5?   #   5?      0  *System.Windows.Controls.DataGridTextColumn  5?      -   5        sequenceColumn??$ Sequence??5?   b   $ 80 6t      	CellStyle# [ 5?   -      
Foreground
 ??  ?1	  +System.Windows.Controls.DataGridBoundColumn 	  Binding 5?   E   ?? *
Sequence+5?   U     5?      -   5        unitsLeftColumn??$ 
Units Left??5?   d   $ 80 6x   # [ 5?   -   
 ??  ? 5?   F   ?? *	UnitsLeft+5?   U   4
  .System.Windows.Controls.DataGridCheckBoxColumn
  5?      -   5        deliverOnCreditColumn??$ Deliver On Credit??5?   t   $	 150 5?   1    5?   P   ?? *DeliverOnCredit+5?   1   5?      5?      ?? 5?      -   5         ??Name$ HistoryGroupBox??5?      $??	10,20,0,0q?60   ??5?      ?? 5?      
???  ??$??	0,10,0,10q?61   ?? 5?   &   .?6+   Subscription history6A   5?      .??5?        -   5        $ History??5?       .  $
??Auto??6H    . $ VisibleWhenSelected=?5?   0    5?      ?? 5?      6"    5       5     -   5        dateFromColumn??$
 Date??5  E   
 ??  ?$	 100 5  ?    5  S   ?? *
DateFrom+# [ 5  ?     5     -	   5        operationStringColumn??$ 	Operation??5  p   
 ??  ?$	 150 6?    6L   ?? *OperationString+6?     5     $ Issue Description??61   $	 300 6L    5     ?? 5  "   .R?5	  &   ?? $
??Auto??6.   $??3q?6<   $
 Wrap=?6G   
????  ?.??6m   ??5
  *   ?? 5  .   $
 IssueString?66   5  /   .??5  8   5  '   5  #   5       5     -
   5        debitValueColumn??$ Debit??5  -   # [ 6<   
 ??  ?$ 80 5  ?    5  G   ?? *
DebitValue+# [  5  ?     5     -   5        creditValueColumn??$ Credit??5  ?   # [ 5  -   
 ??  ?$ 80 6h    5  H   ?? *CreditValue+# [  5  h   5     5     5     5     5     