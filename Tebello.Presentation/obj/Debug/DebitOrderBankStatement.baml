   M S B A M L   `   `   `  ????O  KTebello.Presentation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Subs.PresentationSubs.Presentation  5   	   D @Subs.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null8*clr-namespace:Subs.Data;assembly=Subs.Data	Subs.Data 5   	   ;"clr-namespace:Tebello.PresentationTebello.Presentation  6A   /    )Subs.Presentation.DebitOrderBankStatement   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:Subs.Presentation     5   	   ;Data*clr-namespace:Subs.Data;assembly=Subs.Data     5   	   5local1"clr-namespace:Tebello.Presentation     6A   -   5          ? Title$  DebitOrder processing??5	   	   $??890.265??6'   $??1427.794??68    ? 	Resources 5      (   "Tebello.Presentation.DateConverter   lDateConverter+   %Tebello.Presentation.IntegerConverter  lIntegerConverter(   "Tebello.Presentation.RandConverter  lRandConverter ?? 
TargetType ?? Property ?? Value  
AlignRight  Subs.Data.PaymentDoc  
paymentDoc % !DebitOrderBankStatementViewSource ?? Source ?? Source  Subs.Data.SBDebitOrderDoc  sBDebitOrderDoc  sBDebitOrderViewSource??%?  &	        &	      &	 &     '?? 9     &	 ?     &	 ?     &	 ?     7  &	 #    &	 6    7    5   
   6     5   
   6"     5   
   6   ?? 5   
   " ??6   .?5      ??  ? ??
 ??ح?6+   5      ?? 5   
   " ?6#   .?5      ??  ? ??$ Right=?64   6D   5        5   
   6   ?? 5   
    6I   ?? *DebitOrderBankStatement+8     5   
   6   ?? 5   
    6>   ?? *SBDebitOrder+8   5      .??5      ?? $??0,0,0,0q?6   .@?5   
   ?? $??Manage debitorder users??6   .??5      ?  ? DataContext# [ 6   $	??Top=?6I    ? ColumnDefinitions 5      ?? 5      $	??54* ?6'   ?? 5      $
??127* ?6'   ?? 5       $
??186* ?6'   ?? 5!      $??1055* ?6'   	 ? RowDefinitions	 5#      ?? 5$      $
??Auto ?6$   ?? 5%      $??40 ?6$   .+?5'      &   System.Windows.Controls.DataGrid  
   AutoGenerateColumns
 .     EnableRowVirtualization .    RowDetailsVisibilityMode$ VisibleWhenSelected=?6n   $	??554??6?      CanUserAddRows .  $??4??6?   ??6V   ??    Columns 5(      0  *System.Windows.Controls.DataGridTextColumn  5)      -   5        customerIdColumn1??,  &System.Windows.Controls.DataGridColumn   Header$ Customer Id??5)   g      Width5	  /System.Windows.Controls.DataGridLengthConverter$
 Auto	 6|      
IsReadOnly . 1
  +System.Windows.Controls.DataGridBoundColumn 
  Binding 6H   ?? *
CustomerId+6?   4  .System.Windows.Controls.DataGridCheckBoxColumn  5*      -   5        suspendedColumn??$ 	Suspended??5*   h   $
 Auto	 6{    6J   ?? *	Suspended+6{     5+      -   5        accountHolderColumn??$ Account Holder??5+   l   $
 Auto	 6?    6J   ?? *AccountHolder+6?     5,      -   5        
bankColumn??$
 Bank??5,   Z   $
 Auto	 6h    6A   ?? *Bank+6h     5-      -   5        bankCodeColumn??$ 	Bank Code??5-   b   $
 Auto	 6u    6E   ?? *
BankCode+6u     5.      -   5        accountNoColumn??$ 
Account No??5.   d   $
 Auto	 6x    6F   ?? *	AccountNo+6x     5/      -   5        commentColumn??$ Comment??5/   `   $
 Auto	 6q    6D   ?? *	Comment+6q   51      ?? 52      $??1??6    ?? Orientation$ 
Horizontal=?6*   $??4??6C   .+?53      ?? -	   5        $	??100??53      $??10q?6J   .??6V   Load specific user6j   ?? 54      -
   5        $	??100??54      $??10q?6C   .??6O   
Load all6Y   ?? 55      -   5        $	??100??55      $??10q?6A   .??6M   Add6R   ?? 56      -   5        $	??100??56      $??10q?6B   .??6N   Save6T   ?? 57      -   5        $	??100??57      $??10q?6B   .??6N   Cancel6V   58      59      5:      ?? 5<   
   $??Propose debitors??6   .??5=      ?? $??0,0,0,0q?6   .+?5>      (  "System.Windows.Controls.DatePicker  -   5          Name$ calenderDeliver??5>       ?? Left$ 22??66   
 ?? Top$ 41??6G      SelectedDateFormat$
 Long=?6W   $	??191??6q   ?? 5?      $'??!Select the planned delivery month??6   $ 22??6D   $ 10??6U     5A      -   5          Name$ ProposalDataGrid??5A      
 .   . $	??551??6p   $
??1076??6}   $ 22??5B      $ VisibleWhenSelected=?6(   $	 110??6W      	RowHeight$ 30Q?6h    .     CanUserDeleteRows .  5C        5D      $ Customer Id??6L   $
 Auto	 6a    6-   ?? *
CustomerId+6a     5E      -   5        productNameColumn??$ Product Name??5E   h   $
 Auto	 6~    6H   ?? *ProductName+6~     5F      -   5        subscriptionIdColumn??$ Subscription Id??5F   n   $
 Auto	 6?    6K   ?? *SubscriptionId+6?     5G      -   5        issueIdColumn??$ Issue Id??5G   `   $
 Auto	 6r    6D   ?? *	IssueId+6r     5H      -   5        issueDescriptionColumn??$ Issue Description??5H   r   $
 Auto	 6?    6M   ?? *IssueDescription+6?     5I      -   5        classificationColumn??$ Classification??5I   n   $
 Auto	 6?    6K   ?? *Classification+6?     5J      -   5        subtractColumn??   	CellStyle# [ 5K   *   $ Subtract??6R   $
 Auto	 6d    5J   E   ?? *
Subtract+ ?? 	Converter# [ 5K   d      RowHeaderStyle 5M      ?? 5N      :  4System.Windows.Controls.Primitives.DataGridRowHeader"  6    .?5O      ??  ? ??
 ? ???6<   ?? 5P       ? ??$ 30??67   ?? 5Q       ? ??
 ?  ??6<   ?? 5R       ? ??$ 3q?6A   5S      5U      ?? 5V      -   5         ??Name$ buttonProposeDebitOrder??5V      $ ??Propose debit order values??6:   $	 423??6_   $ 14??6q   $	??167??6?   5W      ?? 5X      -   5        $??Write to XML??5X      $	 688??60   $ 16??6B   $	??167??6R   5Y      ?? 5[      -   5        $??Write to CSV??5[      $	 688??60   $ 60??6B   $	??167??6R   5\      5]      5_      ?? 5`   
   $??Bank statement??6   .??5a      ?? $??0,0,0,0q?6   # [ 6&   .+?5b      ?? -   5        $ 
buttonLoad??5b      $+??%Load a new bank statement from a file??6-   $ 10??6]   $ 10??6n   $	??270??6~   6?     5d      -   5        $ pickerMonth??5d      $??0,0,0,0q?63   $	 178??6D   $ 91??6V   ?? 5e      -   5        $ buttonSelectRange??5e      $	 523??64   $ 75??6F   $??Load the month??6v   $	??270??6?   ?? 5f      -   5        $ buttonNotPosted??5f      $	 523??62   $	 111??6D   $%??Load the month - not posted yet??6s   $	??270??6?   ?? 5h      -   5        $ buttonValidate??5h      $ 10??61   $	 766??6B   $??Validate the payments??6p   ?? 5i      -   5        $ 
buttonPost??5i      $	 264??6-   $	 766??6?   $ ??Post the validated entries??6i   {?.  6?   ?? 5k      $	 820??6   $ 75??6*   .??6:   Balance over period6O   ?? 5m      -   5         ??Name$ textBalanceOverPeriod??5m      $	 960??69   $ 75??6K   $	??132??6[   $??26??6g     ?? TextAlignment$  Right=?6s   6?   ?? 5n      $??}??6   $	??122??6$   $	 427??61   $ 36??6C   $??46??6S   $??80(?6^   ?? 5p      $??Select the month??6   $ 10??63   $ 91??6D     5r      -    5        $ SBBankStatementDataGrid??5r      
 .   . $	??554??6w   $ 10??6?   !   SelectionUnit$! CellOrRowHeader=?6?   $	 187??5s      $
??1371??6$   $ 30Q?61   $" ?? IsSynchronizedWithCurrentItem$
" TrueY?6@    .  #   
IsReadOnly# .  .  ??5r   ?   ??  5v      ?? 5w      "  6    .?5x      ??  ? ??
 ? ???6<   ?? 5y       ? ??$ 30??67   ?? 5z       ? ??
 ?  ??6<   ?? 5{       ? ??$ 3q?6A   5|       5        5?      -!   5        transactionDateColumn??# [ 5?   L   $ Payment date??6t    6?   ?? *TransactionDate+# [    5?      -"   5        allocationNoColumn??$ Allocation No??5?   j   $ SizeToHeader	 6?    6I   ?? *AllocationNo+6?     5?      -#   5        amountColumn??$ Amount??5?   ?   $ SizeToHeader	 6?   # [ 6?    6C   ?? *Amount+# [ 6?     5?      -$   5        referenceColumn??$ 	Reference??5?   d   $	 200	 6w    6F   ?? *	Reference+6w     5?      -%   5        bankPaymentMethodColumn??$ Bank Payment Method??5?   t   $ SizeToHeader	 6?    6N   ?? *BankPaymentMethod+6?     5?      -&   5        customerIdColumn??$ Customer Id??5?   ?   $ SizeToHeader	 6?   # [ 5?   %    5?   G   ?? *
CustomerId+$ ?? Mode$$ TwoWay=?# [ % ?? UpdateSourceTrigger$% 	LostFocus=?5?   %     5?      -'   5        errorMessageColumn??$ Message??5?   j   $
 auto	 6{    6I   ?? *ErrorMessage+6{     5?      $ Posted??6L   $ SizeToHeader	 6\    61   ?? *Posted+6\     5?      $ Payment Transaction Id??6V   $ SizeToHeader	 6v   # [ 5?   %    5?   -   ?? *PaymentTransactionId+5?   %   & ? ContextMenu& 5?      ?? 5?      .@?5?      m? -(   5        $??Find Customer??5?   '   6>   m? 5?      -)   5        $??GoTo Customer??5?   '   6>   m? 5?      -*   5        $??Accept the payment??5?   '   6C   m? 5?      -+   5        $#??Mark as incorrectly deposited??5?   '   6N   m? 5?      -,   5        $??Mark as internal transfer??5?   '   6J   5?      5?      5?      5?      5?      5?      