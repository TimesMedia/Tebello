   M S B A M L   `   `   `  ????[  WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms  5   	   D @Subs.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null8*clr-namespace:Subs.Data;assembly=Subs.Data	Subs.Data 5   	   O KTebello.Presentation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:Tebello.PresentationTebello.Presentation 5   	   5clr-namespace:Subs.PresentationSubs.Presentation 5   P   *   $Tebello.Presentation.CustomerPicker3   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   Owf@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   9my*clr-namespace:Subs.Data;assembly=Subs.Data     5   	   4local"clr-namespace:Tebello.Presentation     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local1clr-namespace:Subs.Presentation     6P     ? Title$  CustomerPickerWindow??5	   	    ? WindowState$ 	Maximized=?6&    ? WindowStartupLocation$ Manual=?6>    ? Left$ 0??6]   
 ? Top$ 20??6f   
???????$	??920??5
      $
??1000??6)   $
??1330??69    ? 	Resources 5        Subs.Data.LedgerDoc2   
ledgerDoc2(  "Tebello.Presentation.RandConverter  lRandConverter+  %Tebello.Presentation.IntegerConverter  lIntegerConverter(  "Tebello.Presentation.DateConverter  lDateConverter  customerViewSource  transactionsViewSource ?? Source ?? Path ?? Source ! InvoicesAndPaymentsViewSource  PaymentViewSource  InvoiceViewSource 	 SubscriptionsViewSource 
 TooMuchTooLittleViewSource  
AlignRight	 ?? 
TargetType
 ?? Property ?? Value:  4System.Windows.Controls.Primitives.DataGridRowHeader??%?  &	        &	      &	 &     &	 9     &	 L     &	 _     7   &	 ?     7   &	 ?     &	 ?     &		 ?     &	
     &	     '  j      5   
   6     5   
   6     5   
   6!     5   
   6   ?? 5   
   6   ?? 5   
    6>   ?? $ Transactions?8   ?? 5   
    6E   ?? $ InvoicesAndPayments?8   ?? 5   
   6   ?? 5   
   6   ?? 5   
   6   ?? 5   
   6   ?? 5   
   "	 ?6#   .?5      ?? 
 ? ??$ Right=?64   6D   5      ?? 5   
   "	  6   .?5       ?? 
 ? ??
 ? ???6,   ?? 5!      
 ? ??$ 30??6'   ?? 5"      
 ? ??
 ?  ??6,   ?? 5#      
 ? ??$ 3q?61   5$      5&      .??5)      ?? -   5         ??Name$ tabControl1??5)   =   $
??Auto??6   $??Stretch=?6   $??Stretch=?6R   $
??Auto??6n   $??0,0,0,0q?6{   .@?5+   
   ?? -   5         ??Name$ 	tabSearch??5+   "   $??Search??6   $??0??65   .??5-      ? $??5q?6    ? RowDefinitions 5.      ?? 5/      $
??auto ?6$   ?? 50      $??* ?6$   .+?52      ?? -   5         ??Name$ SearchGroupBox??52      $Q??KSearch for an existing subscriber by choosing one of the following criteria??63   $	??172??6?   $
??Left=?6?   $??0,0,0,0q?6?   $	??796??53   !   
???????$
??1167??6C   .??54      ?? $
??Auto??6   $
??Auto??6+   $
??Left=?68   .+?55      ?? -   5         ??Name$ buttonSearchCustomerId??55   ^   $??1??6!   $??Search by Customer Id.??61   $??22??6R   $	??147??6~    ?? Left$ 16??6?   
 ?? Top$	 118??6?    ?? 	IsDefault . 56   )   ?? 57      -   5        $ buttonSearchSurname??57   Y   $??1??6!   $??Search by Surname??61   $??22??6M   $	??147??6v   $ 16??6?   $ 7??6?   58      ?? 59      -   5        $ buttonSearchCompany??59   Y   $??1??6!   $??Search by Company??61   $??22??6M   $	??147??6v   $	 197??6?   $ 7??6?   5:      ?? 5;      -   5        $ buttonSearchEmail??5;   W   $??1??6!   $??Search by Email??61   $??22??6K   $	??147??6r   $	 403??6~   $	 118??6?   5<      ?? 5=      -   5        $ buttonSearchInvoice??5=   Y   $??1??6!   $??Search by Invoice??61   $??22??6M   $	??147??6v   $ 16??6?   $ 59??6?   5>      ?? 5?      -	   5        $ 
buttonLoad??5?   _   $??1??6!   $??Load from existing list??61   $??22??6S   $	??163??6s   $	 197??6   $	 118??5@   %   5A      7  1System.Windows.Forms.Integration.WindowsFormsHost  5C      -
   5        $	 208??5C   +   $ 55??6=   $??26??6M   $	??342??6Y      Child. 5D      #   System.Windows.Forms.ComboBox  -   5        comboCompany??"   System.Windows.Forms.Control   Enabled .    DropDownHeight$	 300??5D   O   5E      ?? 5G      -   5        $??	buttonNew??5G   p   $	 956??6!   $ 16??63   $??Capture a new customer??6C   $??22??6d   $	??184??6?   ?? 5H      -   5        $??buttonNewResume??5H   x   $	 956??6!   $ 67??63   $$??Resume capture of new customer??6C   $??22??6l   $	??184??5I   %   {?.  61   ?? 5J      -   5        $??button1??5J   K   $??Write result to XML??6!   $??22??6?   $	??184??6Z   $??Center=?6f   $	 956??5K   E   $	 118??6W   5L      5M      &	   System.Windows.Controls.DataGrid	  5O      -   5         	 Name$ customerDataGrid??5O      $??1??65   $??1??6B    ? DataContext# [ 6R   $ ?? IsSynchronizedWithCurrentItem$
 TrueY?6?    	  AutoGenerateColumns .   	  EnableRowVirtualization . 
???????$
??Left=?5Q      $??0,10,0,0q?5R       	  SelectionUnit$ CellOrRowHeader=?6    	  SelectionMode$ Single=?5S       	  
IsReadOnly .  	  CanUserAddRows .    	  	RowHeight$  30Q?5T   ?   $
??auto??5U      $??Stretch=?6+   ! 	  FrozenColumnCount$! 6??6G   " 	  CanUserSortColumns" . # 	  CanUserReorderColumns# . ??5Q   8   ?? $ 	  RowHeaderStyle$ 5V      ?? 5W      "	  6    .?5X      ?? 
 ? ??
 ?????6<   ?? 5Y      
 ? ??$ 30??67   ?? 5Z      
 ? ??
 ??ح?6<   ?? 5[      
 ? ??$ 3q?6A   5\      ??5^      ?? 5_      .?5`      ?? 
 ? ??$ 6q?6C   ?? 5a      
 ? ??
 ??ح?6G   5b      % 	  Columns% 5d      0
  *System.Windows.Controls.DataGridTextColumn
  5e      -   5        customerIdColumn??,  &System.Windows.Controls.DataGridColumn&   Header$& Customer Id??5e   f   '   
IsReadOnly' . 1  +System.Windows.Controls.DataGridBoundColumn(   Binding( 6G   ?? *
CustomerId+6{   
  5f      -   5        titleColumn??$& Title??5f   \   ( 6B   ?? *Title+6\   
  5g      -   5        initialsColumn??$& Initials??5g   b   ( 6E   ?? *
Initials+6b   
  5h      -   5        firstNameColumn??$& 
First Name??5h   d   ( 6F   ?? *	FirstName+6d   
  5i      -   5        surnameColumn??$& Surname??5i   `   ( 6D   ?? *	Surname+6`   
  5j      -   5        countryNameColumn??$& Country Name??5j   h   ( 6H   ?? *CountryName+6h   
  5k      -   5        phoneNumberColumn??$& Phone Number??5k   h   ( 6H   ?? *PhoneNumber+6h   
  5l      -   5        cellPhoneNumberColumn??$& Cell Phone Number??5l   p   ( 6L   ?? *CellPhoneNumber+6p   
  5m      -   5        liabilityColumn??)   	CellStyle#) [ 5n   5   $& 	Liability??6]   ( 5m   F   ?? *	Liability+* ?? StringFormat$* {0:R ### ### ##0.00}??5n   ]   
  5o      -   5        emailAddressColumn??$& Email Address??5o   j   ( 6I   ?? *EmailAddress+6j   
  5p      -   5        companyNameColumn??$& Company Naam??5p   h   ( 6H   ?? *CompanyName+6h   
  5q      $& CPPaymentTransactionId??6`   ( 6-   ?? * CheckpointPaymentTransactionId+6`   
  5r      #) [ 6?   $& CPPaymentValue??6?   ( 6-   ?? *CheckpointPaymentValue++ ?? 	Converter#+ [ 6?   
  5s      $& CPPaymentDate??6?   ( 6-   ?? *CheckpointPaymentDate+#+ [ 6?   
  5t      $& CPInvoiceDate??6?   ( 6-   ?? *CheckpointInvoiceDate+#+ [ 6?   
  5u      -   5        nationalId1Column??$& National Id 1??5u   h   ( 6H   ?? *NationalId1+6h   
  5v      -   5        nationalId2Column??$& National Id 2??5v   h   ( 6H   ?? *NationalId2+6h   
  5w      -   5        nationalId3Column??$& National Id 3??5w   h   ( 6H   ?? *NationalId3+6h   
  5x      -   5        departmentColumn??$& 
Department??5x   f   ( 6G   ?? *
Department+6f   
  5y      -   5        commentsColumn??$& Comments??5y   b   ( 6E   ?? *
Comments+6b   
  5z      -    5        physicalAddressIdColumn??$& Physical Address Id??5z   t   ( 6N   ?? *PhysicalAddressId+6t   
  5{      -!   5        modifiedByColumn??$& Modified By??5{   f   ( 6G   ?? *
ModifiedBy+6f   
  5|      -"   5        modifiedOnColumn??$& Modified On??5}   5   ( 5|   G   ?? *
ModifiedOn+#+ [ 5}   5   , ? ContextMenu, 5      ?? 5?      .@?5?      m? -#   5        $??ConsolidateSource??5?   '   6B   m? 5?      -$   5        $??ConsolidateTarget??5?   '   6B   m? 5?      -%   5        $??DeliveryAddress??5?   '   {?6^   ?? *	IsEnabled+- ?? ElementName$- Profile??m? 5?      -&   5        $??Destroy customer??5?   '   
. ? Tag$. AuthorityHigh??6_   6s   m? 5?      -'   5        $??	Statement??5?   '   6:   m? 5?      -(   5        $ ??Invoices Reset checkpoints??5?   '   6K   m? 5?      -)   5        $??Liability calculation??5?   '   6F   m? 5?      -*   5        / m?Name$/ Profile??5?   `   $??Profile??6'   {?. 6`   m? 5?      -+   5        $??Select??5?   '   67   m? 5?      -,   5        $??Set password??5?   '   6=   m? 5?      --   5        $??Update??5?   '   67   5?      5?      5?      5?      ?? 5?   
   -.   5        $??Tab_Invoice??5?      $??	Statement??6%   $??1??68   $y?Hidden=?6E   .??5?      ?  5?      ?? 5?      $??70 ?6$   62   ?? 5?      $??40 ?6$   62   ?? 5?      $
??Auto ?6$   64   ?? 5?      $??40 ?6$   62   ?? 5?      $??* ?6$   61   0 ? ColumnDefinitions0 5?      ?? 5?      $	??19* ?6'   ?? 5?      $??6* ?6'   ?? 5?      $
??485* ?6'   ?? 5?      $
??292* ?6'   ?? 5?      $
??510* ?6'   .+?5?      ? -/   5        1 ?Name$1 PaymentPanel??5?      2 ? Orientation$2 
Horizontal=?62   $??Stretch=?6K   $
??Auto??6i   $??0??6x   $??5??6?   $??	0,10,0,10q?6?   .+?5?      ?? $??0,0,20,0q?6!   .+?5?      ?? .??6!   
CustomerId6-   ?? 5?      -0   5        3 ??Name$3 
CustomerId??5?   "   4 ?? TextAlignment$4 Right=?66   5 ?? 
IsReadOnly5 . 6a   5?      (  "System.Windows.Controls.DatePicker  5?      -1   5        6  Name$6 PaymentDatePicker??5?   !   6>   ?? 5?      $??Center=?6   .??67   	Amount:6@   ?? 5?      -2   5        $3 textPaymentLeft??5?      $??Center=?67   $??60??6R   ??5?   !   ?? $ PaymentAmount?#+ [ 7 ?? UpdateSourceTrigger$7 	LostFocus=?8 ?? Mode$8 TwoWay=??? 5?      -3   5        $??comboMethod??5?      $??Center=?6/   $??Payment method??6J   6f   ?? 5?      -4   5        9 ??Name$9 comboPaymentMethod??5?      $	 303??6;   $ 30??6M   $	??167??6]   : ?? DisplayMemberPath$: Value??6i   ; ?? SelectedValuePath$	; Key??6?   $??27??5?   ?   < ?? SelectedValue< 6   ?? $ PaymentMethod?$8 OneWayToSource=?$7 	LostFocus=?5?      ?? 5?      $??Reference type??6   $??Center=?66   6S   ?? 5?      -5   5        $9 comboReferenceType??5?      $	 350??6;   $ 30??6M   $	??167??6]   $: Value??6i   $	; Key??6?   $??27??5?   ?   < 6   ?? $ ReferenceTypeId?$8 OneWayToSource=?$7 	LostFocus=?5?      ?? 5?      $??	Reference??6   $??Center=?61   ?? 5?      -6   5        $??textPaymentReference??5?      $	??208??6;   = ?? TextWrapping$
= Wrap=?5?   5   ??5?   G   ?? $ PaymentReference?$8 OneWayToSource=?$7 	LostFocus=?5?   5   ?? 5?      -7   5        $ buttonPayManually??5?      $??10,0,0,0q?6X   $. AuthorityHigh??6k   .??6?   Pay manually6?   5?      ?? 5?      -8   5        $??1??5?      $??0??6&   $??30??66   $
??Left=?6d   $	??230??6?   $. AuthorityHigh??6?   $??3??6?   .??6?   Reset all checkpoints6?   ?? 5?      $??1??6   $??3??6%   $??Center=?65   $??15(?6R   
??? ???$	??100??6r   .??6~   
Payments6?   ?? 5?      -9   5        $ ButtonAllocate??5?      $??1??61   $??5??6>   $??Right=?6k   $. AuthorityHigh??5?      $	??230??6E   .??6Q   Allocate all the payments6l   	  5?      -:   5        $ PaymentDataGrid??5?      $??2??64   $??5??6A    .  $
??Auto??5?   G   $??Stretch=?6T    .  > 	  CanUserDeleteRows> .   .  . " .  $??	0,10,0,10q?5?   k   $	??250??5?      ??5?   U   ?? # [ % 5?      
  5?      $& TransactionId??6O   ?   Width5  /System.Windows.Controls.DataGridLengthConverter$? 80 6f   ( 6-   ?? *TransactionId+6f   
  5?      $& 	Operation??6K   $	? 125 6^   ( 6-   ?? *	Operation+6^   
  5?      $
& Date??6-   $? 80 6;   ( 5?   =   ?? *Date+#+ [ 4  .System.Windows.Controls.DataGridTemplateColumn  5?      $& 	Reference??61   $	? 150 6D   @   CellTemplate@ 5?      ?? 5?   "   .R?5?   &   ?? $
??Auto??6.   $??3q?6<   $
= Wrap=?6G   .??6[   ??5?   *   ?? 5?   .   $ 
Reference2?66   5?   /   .??5?   8   5?   '   5?   #   5?      
  5?      $& Value??6r   $	? 100 6?   #) [ 6?   ( 6-   ?? *Value+#+ [ 6?   
  5?      $& Unallocated portion??6{   $	? 120 6?   #) [ 6?   ( 6-   ?? *InvoiceBalance+#+ [ 6?   
  5?      $& Unallocated balance??6}   $	? 120 6?   #) [ 6?   ( 6-   ?? *StatementBalance+#+ [ 6?   , 5?      ?? 5?      .@?5?      m? -;   5        $!??Assign refund to payment...??5?   '   $. AuthorityHigh??6h   A ? ToolTip$!A Allocate a historic refund.??6?   m? 5?      -<   5        $??Bounce payment??5?   '   6?   m? 5?      -=   5        $??Highlight allocations??5?   '   6F   m? 5?      ->   5        $??Refund payment??5?   '   $. AuthorityHigh??6T   6h   m? 5?      -?   5        $??Reverse payment??5?   '   6@   m? 5?      -@   5        $#??Select payment for allocation??5?   '   $. AuthorityHigh??6w   6?   m? 5?      -A   5        $??Set as checkpoint??5?   '   $#A Use the current row as input.??6g   $. AuthorityHigh??6?   6?   m? 5?      -B   5        $$??Set checkpoint one day further??5?   '   $A Advance date by one day??6u   $. AuthorityHigh??6?   6?   m? 5?      -C   5        $"??Set as reduced checkpoint...??5?   '   $0A *You will  be prompted for a reduced value.??6r   $. AuthorityHigh??6?   6?   5?      5?      ?? 5?      -D   5        $??3??5?      $??0??6&   $
??Left=?66   $	??230??6Q   $A Invoke input.??5?      $. AuthorityHigh??62   $??3??6]   .??6q   Set a checkpoint for invoices6?   ?? 5?      $??3??6   $??3??6%   $??Center=?65   $??15(?6R   
??? ???$	??100??6r   .??6~   
Invoices6?   ?? 5?      -E   5        $??3??5?      $??4??6'   $??Right=?6\   $	??230??6y   $. AuthorityMedium??6?   .??6?   Create a statement6?   	  5      -F   5        $ InvoiceDataGrid??5      $??4??64   $??5??6A    .  $
??Auto??5     $??Stretch=?6,   $
??auto??6J   $??Stretch=?6X    .  > .   .  . " .  $??	0,10,0,10q?5  ?   $
 TrueY?6?   $??2??6?   ??5   U   ?? # [ B ?? 
GroupStyleB 5     (  "System.Windows.Controls.GroupStyle  5     C   HeaderTemplateC 5     ?? 5  "   .R?5  &   ?? $??30??60   $
??Bold"?6<   
???  ??
???????$??0,20,0,0q?6w   5	  #   5     % 5     
  5     $& TransactionId??6O   $? 80 6f   ( 6-   ?? *TransactionId+6f   
  5     $& Invoice??6K   $? 80 6\   ( 6-   ?? *	InvoiceId+6\   
  5     $& 	Operation??6K   $	? 125 6^   ( 6-   ?? *	Operation+6^   
  5     $
& Date??6-   $? 80 6;   ( 5  =   ?? *Date+#+ [   5     $& 	Reference??61   $	? 150 6D   @ 5     ?? 5  "   .R?5  &   ?? $
??Auto??6.   $??3q?6<   $
= Wrap=?6G   .??6[   ??5  *   ?? 5  .   $ 
Reference2?66   5  /   .??5  8   5  '   5  #   5     
  5!     $& Value??6r   $	? 100 6?   #) [ 6?   ( 6-   ?? *Value+#+ [ 6?   
  5"     $& Invoice balance??6{   $	? 100 6?   #) [ 6?   ( 6-   ?? *InvoiceBalance+#+ [ 6?   
  5#     $& Statement balance??6}   $	? 110 6?   #) [ 6?   ( 6-   ?? *StatementBalance+#+ [ 6?   , 5%     ?? 5&     .@?5'     m? -G   5        $??Allocate payment??5'  '   $. AuthorityHigh??6`   6t   m? 5(     -H   5        $??Deallocate payment??5(  '   $. AuthorityHigh??6d   6x   m? 5)     -I   5        $ ??Reverse  writeoff of money??5)  '   $. AuthorityHigh??6o   6?   m? 5*     -J   5        $??Recreate the invoice??5*  '   $. AuthorityHigh??6d   6x   m? 5+     -K   5        $??View Subscriptions??5+  '   6C   m? 5,     -L   5        $ ??Write off a certain amount??5,  '   $. AuthorityHigh??6g   6{   5-     5/     50     51     ?? 52  
   -M   5        $ tabSubscription??52     $??Subscription??6+   $??2??6A   $y?Visible=?6N   .??53     ? # [ 6   0 54     ?? 55     $??16 ?6'   ?? 56     $	??437 ?6'   ?? 57     $
??Auto ?6'    59     ?? 5:     $??* ?6$   .+?5>     ?? $??0??6   $??2??6-   .+?5?     ?? $??Center=?6   .??69   INVOICES AND PAYMENTS6P   	  5@     -N   5        $ SubscriptionDataGrid??5@      .   . $??0,0,0,0q?5A  #   D 	  RowDetailsVisibilityMode$D VisibleWhenSelected=?64   $
??Left=?6c   $  30Q?6~    .  > .  $
??Auto??5B  N   $
??auto??6[   $??Stretch=?6i   ??5@  x   ?? # [	 % 5C     
  5D     $& SubscriptionId??6T   $	? 100 6l   #) [ 5E  5   ( 5D  1   ?? *SubscriptionId+5E  5   
  5F     $& Status??6L   $	? 100 6\   ( 61   ?? *Status+6\   
  5G     $& Product??6Q   $	? 150 6b   ( 61   ?? *ProductName+6b   
  5H     $& Value??5I  5   $	? 100 6D   #) [ 6P   ( 5H  1   ?? *Value+#+ [ 5I  P   , 5K     ?? 5L     .@?5M  "   m? -O   5        $??Status and history??5M  +   6G   m? 5N  "   -P   5        $??Deliver on credit??5N  +   $. AuthorityMedium??6d   6z   5P     5R     5S     9  3Subs.Presentation.SubscriptionStatusDisplayControl2  5X     -Q   5        E  Name$'E !gSubscriptionStatusDisplayControl??5X  ;   $??2??6f   $??11,0,0,0q?6v   5Z     5[     ?? 5]  
   $"??Over payments and deliveries??6   $??3??69   $??23??6F   $??Bottom=?6R   .??5^     ? 0 5_     ?? 5`     $	??76* ?6'   ?? 5a     $
??195* ?6'   ?? 5b     $??7* ?6'   ?? 5c     $??9 ?6'   ?? 5d     $??91 ?6'   ?? 5e     $
??278* ?6'   ?? 5f     $
??278* ?6'   ?? 5g     $	??100 ?6'   ?? 5h     $
??278* ?6'    5j     ?? 5k     $??40 ?6$   ?? 5l     $??* ?6$   .+?5n     ?? -R   5        $??0??5n     $??Who paid too much???6)   $??3??6g   $??40??6{   $??Bottom=?6?   ?? 5o     -S   5        F ??Name$F label2??5o  K   $??3??6   $??In excess of??6(   $??28??6?   $??2??6[   $??0,6,0,7q?6o   ?? 5p     -T   5        $3 textOverPaid??5p     $??5??60   $??23??6@   $	??120??6L   $4 Right=?6X   $
??Left=?6n   $	??0,9q?6?   $??1??6?   ?? 5r     -U   5        $??6??5r     $!??Deliveries without payment???6)   6O   ?? 5s     -V   5        $F label3??5s  K   $??7??6   $??In excess of??6(   $??28??6?   $??0,6,0,7q?6[   ?? 5t     -W   5        $3 textOverDelivered??5t     $??8??65   $??23??6E   $	??120??6R   $4 Right=?6^   $
??Left=?6t   $	??0,9q?6?   $??1??6?   	  5v     -X   5        $ PaidTooMuchDataGrid??5v     $??1??68   $??9??6E    .  .  $  30Q?6?    . $
??auto??6?   $??Stretch=?6?   ??5w     ?? # [
 % 5x     
  5y     $& Customer??6L   $
? Auto 6_   ( 6-   ?? *
CustomerId+6_   
  5z     $& Surname??6I   $
? Auto 6[   ( 6-   ?? *	Surname+6[   
  5{     $& Company??6M   $
? Auto 6_   ( 6-   ?? *CompanyName+6_   
  5}     #) [ 5~  5   $& Overpayment??6]   $
? Auto 6s   ( 5}  -   ?? *Discrepancy+#+ [ 5~  s   , 5?     ?? 5?     .@?5?     m? -Y   5        $??Invoices??5?  '   69   5?     5?     	  5?     -Z   5        $ OverDeliveredDataGrid??5?     $??1??6:   $??9??6G    .  .  $  30Q?6?   $y?Hidden=?6?   ??5?     ?? # [
 % 5?     
  5?     $& Customer??6L   $
? Auto 6_   ( 6-   ?? *
CustomerId+6_   
  5?     $& Surname??6I   $
? Auto 6[   ( 6-   ?? *	Surname+6[   
  5?     $& Company??6M   $
? Auto 6_   ( 6-   ?? *CompanyName+6_   
  5?     #) [ 5?  5   $& Delivered without payment??6]   $
? Auto 6?   ( 5?  -   ?? *	Liability+#+ [ 5?  ?   , 5?     ?? 5?     .@?5?     m? -[   5        $??Invoices??5?  '   69   5?     5?     5?     5?     ?? 5?  
   -\   5        $ 
tabRunning??5?  1   $??Liability calculation??6   $??4??6F   .??5?     ?  5?     ?? 5?     $??40 ?6$   ?? 5?     $??* ?6$   .+?5?     ? .+?5?     ?? -]   5        $ ButtonRunningToXML??5?     .??6Z   Write to XML6h   ?? 5?     .??6   Your current liability is:68   ?? 5?     -^   5        $3 textCurrentLiability??5?     $??	10,0,10,0q?6<   5 . 6c   ?? 5?     -_   5        $. AuthorityHigh??5?  G   .??6r   Synchronise liability6?   5?     	  5?     -`   5        $ LiabilityDataGrid??5?      .   . $
??auto??6q   $??Stretch=?6   $??1??6?   $??0,0,0,0q?5?  #   $D VisibleWhenSelected=?64   $
??Left=?6c   % 5?     
  5?     $& TransactionId??6.   $? 80 6E   ( 6P   ?? *TransactionId+
  5?     $
& Date??6.   $? 80 6<   ( 6G   ?? *Date+#+ [ 
  5?     $& 	Operation??6K   $	? 105 6^   ( 6-   ?? *	Operation+6^   
  5?     $& 	InvoiceId??6K   $	? 105 6^   ( 6-   ?? *	InvoiceId+6^   
  5?     $& SubscriptionId??6P   $	? 105 6h   ( 6-   ?? *SubscriptionId+6h   
  5?     $& Value??6r   $? 70 6?   #) [ 6?   ( 6-   ?? *Value+#+ [ 6?   
  5?     $& Balance??6t   $? 70 6?   #) [ 6?   ( 6-   ?? *	Balance+#+ [ 6?   5?     5?     5?     ?? 5?  
   -a   5        $ tabXps??5?  %   $??	XPSViewer??6   $??3??65   $y?Hidden=?6B   .??5?     Z? -b   5        G Z?Name$G DocumentViewer1??5?     $
??Auto??67   $
??Auto??6E   5?     5?     5?     