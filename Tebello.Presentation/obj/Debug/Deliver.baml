   M S B A M L   `   `   `  ????O  KTebello.Presentation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:Tebello.PresentationTebello.Presentation  5   	   D @Subs.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null8*clr-namespace:Subs.Data;assembly=Subs.Data	Subs.Data 6:   M ISystem, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089M3clr-namespace:System.ComponentModel;assembly=SystemSystem.ComponentModel 6p   "    Tebello.Presentation.Deliver   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:Tebello.Presentation     5   	   9my*clr-namespace:Subs.Data;assembly=Subs.Data     6:   Bcm3clr-namespace:System.ComponentModel;assembly=System     6p     ? Title$  Deliver??5   	   $	??450??6   $
??1169??6&    ? 	Resources 5	        Subs.Data.DeliveryDoc   deliveryDoc  deliveryRecordViewSource ?? Source ?? Path ?? Source(   "Tebello.Presentation.RandConverter  lRandConverter(   "Tebello.Presentation.DateConverter  lDateConverter  
AlignRight ?? 
TargetType ?? Property ?? Value,  &System.ComponentModel.BackgroundWorker  backgroundWorker   WorkerReportsProgress  backgroundWorkerPost??%a  &	        &	      7   &	 I     &	 \     &	 o     &	 ?     &	 ?       5
   
   6   ?? 5   
    6@   ?? $ DeliveryRecord?8     5   
   6     5   
   6   ?? 5   
   " ?6#   .?5      ??  ? ??$ Right=?64   6D   5        5   
   -   5         . 5   F     5   
   -   5         . 5   J   5      .??5      ? 	 ? DataContext#	 [ 6   
 ? RowDefinitions
 5   
   ?? 5      $
??auto ?6   ?? 5      $??* ?6   ?? 5      $
??auto ?6   .+?5!   
   ?  ? ColumnDefinitions 5"      ?? 5#      $??4* ?6#   ?? 5$      $??2* ?6#   ?? 5%      $??* ?6#   .+?5'      ??  ?? Orientation$ 
Horizontal=?6   .+?5(      ?? -   5        $)??#Generate an issue specific proposal??5(   0   $
??Left=?6^   ?? 5)      -   5         ??Name$ labelProduct??5)      
??? ???.??6B   	Product6K   ?? 5*      -   5        $ 
labelIssue??5*      
??? ???.??6@   Issue6G   5+      ?? 5,      -   5        $??1??5,      $1??+Generate a proposal for all active products??6B   $	??300??6x   ?? 5-      -   5        $??buttonValidateProposal??5-   d   $??2??6   $??Load an existing proposal??6&   $??Right=?6?   5.      &   System.Windows.Controls.DataGrid  50   
   -   5        $??deliveryRecordDataGrid??51         AutoGenerateColumns .     EnableRowVirtualization . $
??Auto??50   N   $??Stretch=?6\   $ ?? IsSynchronizedWithCurrentItem$
 TrueY?51   1      	RowHeight$ 30Q?52         RowDetailsVisibilityMode$ VisibleWhenSelected=?6%      CanUserDeleteRows .    CanUserAddRows .  $??1??6?   ??50   x   ??    RowHeaderStyle 53      ?? 54      :  4System.Windows.Controls.Primitives.DataGridRowHeader"  6   .?55      ??  ? ??
 ? ???64   ?? 56       ? ??$ 30??6/   ?? 57       ? ??
 ?  ??64   ?? 58       ? ??$ 3q?69   59         Columns 5;      4  .System.Windows.Controls.DataGridCheckBoxColumn  5<      -	   5        
skipColumn??,  &System.Windows.Controls.DataGridColumn   Header$
 Skip??5<   [      Width5	  /System.Windows.Controls.DataGridLengthConverter$ 30	 6i   1
  +System.Windows.Controls.DataGridBoundColumn 
  Binding 6=   ?? $
 Skip?6i   0  *System.Windows.Controls.DataGridTextColumn  5=      $ Issue description??6O   $
 auto	 6j    6%   ?? $ IssueDescription?6j     5>      -
   5        subscriptionIdColumn??$ Subscription Id??5>   k   $ SizeToHeader	 6?    6C   ?? $ SubscriptionId?6?     5?      -   5        payerColumn??$ Payer Id??5?   Y   $ SizeToHeader	 6k    6:   ?? $ Payer?6k     5@      -   5        
DebitOrder??$ 
Debitorder??5@   a   $ SizeToHeader	 6u      
IsReadOnly .    CanUserSort .  6=   ?? $ 
DebitOrder?6?     5A      -   5        receiverIdColumn??$ Receiver Id??5A   c   $ SizeToHeader	 6x    6?   ?? $ 
ReceiverId?6x     5B      -   5        titleColumn??$ Title??5B   Y   $ SizeToHeader	 6h    6:   ?? $ Title?6h     5C      -   5        initialsColumn??$ Initials??5C   _   $ SizeToHeader	 6q    6=   ?? $ Initials?6q     5D      -   5        surnameColumn??$ Surname??5D   ]   $	 100	 6n    6<   ?? $ Surname?6n     5E      -   5        phoneNumberColumn??$ Phone Number??5E   e   $ SizeToHeader	 6{    6@   ?? $ PhoneNumber?6{     5F      -   5        validationStatusColumn??$ Validation Status??5F   o   $
 Auto	 6?    6E   ?? $ ValidationStatus?6?     5G      -   5        companyColumn??$ Company??5G   ]   $
 Auto	 6n    6<   ?? $ Company?6n     5H      -   5        deliveryaddressColumn??$ DeliveryAddressId??5H   o   $
 Auto	 6?    6D   ?? $ DeliveryAddressId?6?     5I      -   5        descriptionColumn??$ Description??5I   e   $ SizeToHeader	 6z    6@   ?? $ Description?6z     5J      -   5        unitsPerIssueColumn??$ Units Per Issue??5J   i   $ SizeToHeader	 6?      	CellStyle# [ 6?    6B   ?? $ UnitsPerIssue?6?     5K      -   5        unitPriceColumn??$ 
Unit Price??5K   ?   $ SizeToHeader	 6?   # [ 6?    6>   ?? $ 	UnitPrice? ?? 	Converter# [ 6?     5L      -   5        widthColumn??$ Width??5L   Y   $ SizeToHeader	 6h   # [ 6}    6:   ?? $ Width?6}     5M      -   5        expirationDateColumn??$ Excpiration Date??5N   )   $ SizeToHeader	 6C    5M   C   ?? $ ExpirationDate?# [ 5N   C     5O      -   5        emailAddressColumn??$ Email Address??5O   g   $
 Auto	 6~    6A   ?? $ EmailAddress?6~    ? ContextMenu 5Q      ?? 5R      .@?5S      m? -   5        $??Subscription transactions??5S      6B   5T      5V      ?? 5[   
   $	??150??6   $??2??6   .+?5]      ??  ?? Left$ 25??6   .??6%   <:Validate and post deliveries and save the XML instructions6a   ?? 5^      -   5        $??buttonValidate??5^   ?   $??Validate the proposal??6   $ 25??6S   
  ?? Top$  78??6d   $??23??6u   $	??150??6?   ? 5_      -   5        ! ?Name$! ProgressBar1??5_      $??20??60   $ 25??6<   $  38??6M   $	??343??6]   " ? Minimum$" 0Q?6i   # ? Maximum$	# 100Q?6u   6?   ?? 5`      -   5        $??
buttonSkip??5`   t   $??Skip the marked entries??6   $ 25??6E   $	  120??6V   $??22??6h   $	??150??6?   ?? 5b      -   5        $??
buttonPost??5b   l   $
??Post??6   $	 218??6%   $  78??67   $??22??6`   $	??150??6~   ?? 5c      -    5        $??buttonGenerateXML??5c   ?   $??Save by deliverymethod??6   $	 218??68   $	  120??6J   $??23??6   $	??150??6?   ?? 5d      $	 469??6   .??6&   Format delivery lists for6A   ?? 5e      -!   5        $??checkPayers??5e   V   $??Payers??6   $	 636??6(   $  7??6:   $??16??6J   $ d? 	IsChecked$
$ TrueY?6i   $??50??6z   ?? 5f      -"   5        $??checkNonPayers??5f   X   $	 711??6   $  8??6)   $??	NonPayers??68   $??16??6L   $
$ TrueY?6n   ?? 5g      -#   5        $??buttonFormatCourierList??5g   u   $??Courier list??6   $	 478??6-   $  38??6?   $??23??6i   $	??307??6?   ?? 5h      -$   5        % ??Name$% 	CreateXSD??5h   q   $??
Create XSD??6   $	 827??6+   $  38??6=   $??23??6e   $	??307??6?   ?? 5j      -%   5        $??buttonFormatMagMail??5j   m   $??Mag mail??6   $	 478??6)   $  78??6;   $??22??6a   $	??307??6?   ?? 5k      -&   5        $ ??buttonFormatRegisteredMail??5k   {   $??Registered mail??6   $	 827??60   $  82??6B   $??22??6o   $	??307??6?   $??0q?6?   ?? 5m      -'   5        $ ??buttonFormatCollectionList??5m   X   $??Collection list??6   $??23??6L   $	??307??6z   $	 478??6?   $	  117??6?   5n      5p      5s      