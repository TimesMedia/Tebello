   M S B A M L   `   `   `  ????O  KTebello.Presentation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:Tebello.PresentationTebello.Presentation  5   	   D @Subs.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null8*clr-namespace:Subs.Data;assembly=Subs.Data	Subs.Data 6:   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R8clr-namespace:System.ComponentModel;assembly=WindowsBaseSystem.ComponentModel 6r   5clr-namespace:Subs.PresentationSubs.Presentation  6?   0    *Tebello.Presentation.AdministrationProduct   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:Tebello.Presentation     5   	   ;Data*clr-namespace:Subs.Data;assembly=Subs.Data     6:   Hscm8clr-namespace:System.ComponentModel;assembly=WindowsBase     6r   2local1clr-namespace:Subs.Presentation     6?     ? Title$  AdministrationProduct??5   	   $	??507??6'   $	??800??64    ? 	Resources 5	        Subs.Data.ProductDoc   
productDoc6   0Tebello.Presentation.DeliveryOptionsToDictionary   lDeliveryOptionsToDictionary  product2ViewSource ?? Source ?? Source  issueViewSource ?? SortDescriptions+  %System.ComponentModel.SortDescription   PropertyName  baseRateViewSource*  $System.Windows.Controls.DataGridCell ?? 
TargetType ?? Triggers P? Property	 P? Value
 ?? Property ?? Value  
AlignRight  WrappedColumnHeaderStyle=  7System.Windows.Controls.Primitives.DataGridColumnHeader/   )Tebello.Presentation.SubscriptionCategory  lSubscriptionCategory(   "Tebello.Presentation.DateConverter  lDateConverter(   "Tebello.Presentation.RandConverter 	 lRandConverter5	   /Tebello.Presentation.SubscriptionMediumInt2Type 
 lSubscriptionMediumInt2Type??%?  &	        &	      &	 &     7   &	 T     7  &	 ?     7  '      &	 ?    &	     '  ?    '?? 1    '?? ?    &	 ?    &	 ?    &		 ?    &	
       5
   
   6     5   
   6,   ?? 5   
    6:   ?? *
Product2+8   ?? 5   
    67   ?? *Product2_Issue+8    5        5      $
 Year??6&     5      $ No??6&   5      ?? 5   
    6:   ?? *Product2_BaseRate+8   ?? 5   
   "  6    5      P? 5       ?@ 
IsSelected	 . . ?5      ?? 
 ? ??
 ? ???63   ?? 5      
 ? ??
 ?   ?63   5      5      ?? 5   
   " ?6#   .?5       ?? 
 ? ??$ Right=?64   5!      ?? 5#   
   "  61   .?5$      ?? 
 ? ?? 5%      ?? 5&      .R?5'      ?? $
??Wrap=?6$   ??68   ?? 5(      5*      ?? 5+      
 ? ??
 ?????6+   5-      ?? 5/   
   "  6   .?50      ?? 
 ? ??
 ?????6+   ?? 51      
 ? ??$ 3q?6(   52      ?? 54   
   " ??6   .?55      ?? 
 ?@??BorderThickness$ 4q?66   56      ?? 5<   
   " ??6   .?5=      ?? 
 ? ??
 ?????6+   5>        5@   
   6%     5A   
   6     5B   
   6   	  5C   
   6+   5E      .??5G      ?? -   5         ??Name$ 
TabControl??5G      $??0,0,-8,0q?6%   .@?5H   
   ?? -   5         ??Name$ 
ProductTab??5H      $??Product??6&    ? DataContext# [ 67   .??5I      ? $??0,0,4,-2q?6    ? ColumnDefinitions 5J      ?? 5K      $
??114* ?6'   ?? 5L      $
??133* ?6'   ?? 5M      $
??250* ?6'   ?? 5N      $
??297* ?6'    ? RowDefinitions 5Q      ?? 5R      $
??Auto ?6$   ?? 5S      $
??Auto ?6$   ?? 5T      $
??Auto ?6$   ?? 5U      $
??Auto ?6$   .+?5Z      &
   System.Windows.Controls.DataGrid
  -   5         
 Name$ product2DataGrid??5Z       
  RowDetailsVisibilityMode$ VisibleWhenSelected=?65    
  EnableRowVirtualization .  
  AutoGenerateColumns .  $??Stretch=?5[   5   $
??Auto??6S    
  AlternatingRowBackground
 ????? 
  CanUserDeleteRows .   
  CanUserAddRows .   
  SelectionUnit$ CellOrRowHeader=?5\   0   $ ?? IsSynchronizedWithCurrentItem$
 TrueY?6P    
  SelectionMode$ Single=?6u   $??0??5]      $??0??6'   $??4??67   $	??Top=?6K   $??50??5^   ?    J? RenderTransformOrigin$ 	0.475,0.4&?6?   ??5Z   d   ??  
  RowHeaderStyle 5_      ?? 5`      :  4System.Windows.Controls.Primitives.DataGridRowHeader"  6    .?5a      ?? 
 ? ??
 ? ???6<   ?? 5b      
 ? ??$ 30??67   ?? 5c      
 ? ??
 ?  ??6<   ?? 5d      
 ? ??$ 3q?6A   5e       
  Columns 5g      0  *System.Windows.Controls.DataGridTextColumn  5h      ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6-      Header$ Status??6:       CanUserSort  .  !   
IsReadOnly! . 1  +System.Windows.Controls.DataGridBoundColumn"   Binding" 6J   ?? *Status+# ?? Mode$# OneTime=?6?   4  .System.Windows.Controls.DataGridTemplateColumn  5i      $ Product name??61   $	 290 6G     . $   CanUserReorder$ .  %   SortMemberPath$% ProductName??6}   ! . &   CellTemplate& 5j      ?? 5k   "   .R?5l   &   ?? -   5        $
??Auto??5l   .   $??3q?6<   ' ?? TextWrapping$
' Wrap=?6G   .??6   ??5m   *   ?? 5n   .   ( ?? Path$( ProductName?66   ) ?? NotifyOnValidationError) . * ?? UpdateSourceTrigger$* 	LostFocus=?6h   + ?? ValidationRules+ 5o   2   5  /System.Windows.Controls.ExceptionValidationRule  5p   6   5r   /   .??5s   8   5t   '   5u   #   5w        5y      $ SizeToHeader 6-   $ Medium??6B   ! . " 6R   ?? *Medium+, ?? 	Converter#, [
 5z   -     5|      -   5        displaySequenceColumn??$ 65 5|   L   $ Display sequence??6W   -   HeaderStyle#- [ 5}   -   .   	CellStyle#. [ 6f   " 5|   q   ?? *DisplaySequence+$# TwoWay=?5}   f     5      -   5        expirationDurationColumn??$ 65 5   O   #. [ 6?   " 6Z   ?? *ExpirationDuration+ 5?      ?? 5?   "   $??Expiration duration??6,   / ? ToolTip$Q/ KThe number of days that a subscription is expired before it gets cancelled.??5?   1   $
??Wrap=?5?   1   5?   #   5?        5?      -   5        defaultNumberOfIssuesColumn??$ 60 5?   R   $ Default number of issues??6]   #- [ 5?   -   #. [ 6f   " 5?      ?? *DefaultNumberOfIssues+5?   f     5?      -   5        weightColumn??$ SizeToHeader 5?   C   $ Weight??6X   #. [ 6?   " 6h   ?? *Weight+6?     5?      -	   5        lengthColumn??$ 50 5?   C   $ 	Length mm??6N   #. [ 6|   #- [ 6?   " 6a   ?? *Length+6?     5?      -
   5        wydteColumn??$ 50 5?   B   $ Width mm??6M   #. [ 6y   #- [ 6?   " 6_   ?? *Wydte+6?     5?      -   5        hoogteColumn??$ 50 5?   C   $ 	Height mm??6N   #. [ 6|   #- [ 6?   " 6a   ?? *Hoogte+6?   4  .System.Windows.Controls.DataGridCheckBoxColumn  5?      -   5        allowMultipleCopiesColumn??$ 60 5?   T   $ Allow multiple copies??6_   #- [ 5?      " 5?   ~   ?? *AllowMultipleCopies+5?        5?      -   5        classificationColumn??$
 Auto 5?   K   $ Classification??6Y   #. [ 6?   ! . " 6q   ?? *Classification+5?   -   5?      ?? 5?      -   5        0 ??Name$0 ProductImage??5?      $??0??6/   $??3??6<   $??Stretch=?6g   $??0??6?   $
??Auto??6?   $??10,0,0,0q?6?   $	??Top=?6?   ??6L   ?? *	Picture+6?   ?? 5?      -   5        1 ??Name$1 buttonAddPicture??5?      $??Add picture??64   $??1??6L   $??0??6Y   $??0,10,0,0q?6i   $??90??6{   $
??Left=?6?   6?   ?? 5?      -   5        $1 buttonAddProduct??5?      $??Add a product??64   $??90??6M   $??1??6X   $??2??6e   $??0,10,0,0q?6u   $??Right=?6?   6?   ?? 5?      2 ?? Orientation$2 Vertical=?6   $??2??64   $??0??6A   $??0,10,0,0q?6Q   $	??Top=?6c   $??2??6{   $	??150??6?   .+?5?      ?? $
??Name??6   $??25??6,   ?? 5?      -   5        3 ??Name$
3 Naam??5?      $??30??6-   4 ?? 	MaxLength$	4 200??6c   ??69   ?? *ProductName+$# TwoWay=?6c   ?? 5?      $??Heading??6   $??25??6/   ?? 5?      -   5        $3 Heading??5?      $??70??60   "5 ?? VerticalScrollBarVisibility$5 Visible=?6b   ??. $
' Wrap=?6?   ??6<   ?? *	Heading+$# TwoWay=?6?   5?      ?? 5?      $2 Vertical=?6   $??2??64   $??2??6A   $??2??6Q   $??0,10,0,0q?6e   $	??Top=?6w   $	??150??6?   .+?5?      ?? $??Description??6   $??25??62   $??5,0,0,0q?6>   ?? 5?      -   5        $3 Description??5?      $	??125??63   $??Stretch=?6@   $
??auto??6^   $??10,0,0,0q?6k   $5 Visible=?5?      6 ?? AcceptsReturn6 . 7 ?? AutoWordSelection7 . ??. $
' Wrap=?5?   B   .??6z   ??5?      ?? 5?      $( ProductDescription?6&   ) . $* 	LostFocus=?6_   $# TwoWay=?6   + 5?   "     5?   &   5?      .??5?   (   5?      5?      ?? 5?      $2 Vertical=?6   $??3??64   $??0??6A   $??
41,10,41,0q?6Q   $??2??6e   .+?5?      ?? $??Valid delivery options??6   .   (Subs.Presentation.DeliveryOptionsControl  5?      -   5        .   (Subs.Presentation.DeliveryOptionsControl8   	LostFocus$8 DeliveryOptionsLostFocus??5?   1   $	??100??5?   4   $
??auto??6B   $??7,0,0,0q?5?   V   9   DeliveryOptions9 5?      ?? 5?      $( DeliveryOptions?6&   ) . $* 	LostFocus=?6\   $# TwoWay=?6|   + 5?   "     5?   &   5?      5?      ?? 5?      $??Default delivery option??6   $??37??6>   ?? 5?      -   5        : ??Name$: DefaultDeliveryOption??5?      ; ?? DisplayMemberPath$; Value??5?      < ?? SelectedValuePath$	< Key??63   $
??auto??5?   [   $??7??6h   $ 0.5,0.5&?6s   ??5?      ?? *DeliveryOptions+#, [ = ?? SelectedValue= 5?      ?? $( DefaultDeliveryOption?$# TwoWay=?z?5?      V? 5?      .~?5?   "   ?? > ?? ScaleY$> -1Q?61   ?? 5?   "   ?? 5?   "   T? 5?   "   5?      5?      5?      ?? 5?      $2 Vertical=?6   $??3??64   $??2??6A   $??	20,0,0,48q?6Q   .+?5?      ?? $??Web classification??6   $??	10,10,0,0q?69   0   *Subs.Presentation.WebProductClassification  5?      -   5        $	??100??5?   6   $
??auto??6D   $??15,0,0,0q?6u   ?   Classification? 5?      ?? 5?      $( 	Category1?6&   ) . $* 	LostFocus=?6V   $# TwoWay=?6v   + 5?   "     5?   &   5?      5?      5?      ?? 5?      $2 Vertical=?6   $??3??64   $??3??6A   $??10q?6Q   $??Stretch=?6]   $
??auto??6{   .+?5?      ?? $??Medium??6   $??29??6-   $	??100??69   $
??Left=?6E   $??5,0,0,0q?6`   ?? 5?      -   5        $	< Key??5?      $; Value??65   $
??auto??6O   $??
10,5,20,10q?6\   = 5?      ?? $( Medium?$# TwoWay=???5?      ?? # [ $( SubscriptionMedium?5?   \   ?? 5?      $??Classification??6   $??5,0,0,0q?65   ?? 5?      -   5        $3 textClassification??5?      $??27??6:   $	??210??6F   $
' Wrap=?6   $4 20??6?   ??6R   ?? *Classification+$# TwoWay=?6?   ?? 5?      $2 
Horizontal=?6!   $??0,30,0,0q?6:   .+?5?      ?? -   5        $1 buttonSubmit??5?   !   $??Submit??67   $??70??6H   $??	10,0,0,0 q?6u   ?? 5?      -   5        $
1 Exit??5?   !   $??Exit without changes??6/   $	??135??6N   $??20,0,0,0q?6m   $??23??6   5?      5?      5?      5?      ?? 5?   
   -   5        $ TabIssue??5?      $??Issue??6$   .??5?      ? # [ 6    5?      ?? 5?      $??* ?6$   ?? 5?      $
??auto ?6$    5?      ?? 5?      $	??122 ?6'   ?? 5?      $??7 ?6'   ?? 5?      $	??607 ?6'   ?? 5      $	??100 ?6'   .+?5     
  -   5        $ issueDataGrid??5      .   . $
??auto??6m   $??Stretch=?6{   $ VisibleWhenSelected=?5     $??Stretch=?6L   $
??Auto??6j   # [ 6x    .  .  $ CellOrRowHeader=?5  M   $
 TrueY?6m   $??3??6?   ??5  ?   ??  5     ?? 5     "  6    .?5     ?? 
 ? ??
 ? ???6<   ?? 5     
 ? ??$ 30??67   ?? 5	     
 ? ??
 ?  ??6<   ?? 5
     
 ? ??$ 3q?6A   5      5       5     -   5        issueIdColumn??$ Issue??5  `   $ 40 6o   #. [ 6{   " 6D   ?? *	IssueId+6{     5     $ Issue description??61   $
 auto 6L     . $ .  $% Issue??6?   & 5     ?? 5  "   .R?5  &   ?? -   5        $
??Auto??5  .   $??3q?6<   $
' Wrap=?6G   .??6~   ??5  *   ?? 5  .   $( IssueDescription?66   ) . $* 	LostFocus=?6m   + 5  2     5  6   5  /   .??5  8   5  '   5  #   5       5#     -   5        
yearColumn??$
 Year??5#  Z   $ 40 6h   #. [ 6t   " 6A   ?? *Year+6t     5$     -    5        noColumn??$ No??5$  V   $ SizeToHeader 6b   #. [ 6x   " 6?   ?? *No+6x     5%     -!   5        sequenceColumn??$ Sequence??5%  b   $ SizeToHeader 6t   ! . #. [ 5&  1   " 5%  E   ?? *
Sequence+5&  1     5(     -"   5        PromotionPriceColumn??#- [ 5)  1   $ Promotion price??6i   $ 60 6?   #. [ 6?   " 5(  K   ?? *PromotionPrice+#, [	 5)  ?     5+     -#   5        stockProducedColumn??$ Stock produced??5+  l   $ 60 6?   #- [ 5,  1   #. [ 6i   " 5+  J   ?? *StockProduced+5,  i     5-     -$   5        stockDeliveredColumn??$ Stock delivered??5-  n   ! . #- [ 5.  1   $ 60 6i   #. [ 6u   " 5-  K   ?? *StockDelivered+5.  u     5/     -%   5        stockLostColumn??$ 
Stock lost??5/  d   ! . #- [ 50  1   $ 40 6j   #. [ 6v   " 5/  F   ?? *	StockLost+50  v     51     -&   5        stockReturnedColumn??$ Stock returned??51  l   ! . #- [ 52  -   $ 60 6e   #. [ 6q   " 51  J   ?? *StockReturned+52  q     53     -'   5        startDateColumn??$ 
Start Date??53  J   $	 100 6^   & 54     ?? 55  "   .R?56  &   (  "System.Windows.Controls.DatePicker  @   SelectedDate@ 61   ?? *	StartDate+$# TwoWay=?) . A ?? ValidatesOnExceptionsA . $* 	LostFocus=?57  #   59       5:     -(   5        endDateColumn??$ End Date??5:  H   $	 100 6Z   & 5;     ?? 5<  "   .R?5=  &     @ 61   ?? *	EndDate+$# TwoWay=?) . A . $* 	LostFocus=?5>  #   5@     5B     ? 5D     $??1??6   $??0??6$   $??0,10,0,0q?64   $??3??6F    5E     ?? 5F     $
??auto ?6(    5H     ?? 5I     $??* ?6+   ?? 5J     $??* ?6+   ?? 5K     $??* ?6+   .+?5O     ?? -)   5        $1 buttonAddIssue??5O     $??0??65   $??Add an issue??6E   $??30??6\   $??75??6h   6s   ?? 5P     -*   5        $1 buttonSubmitIssue??5P     $??1??68   $??Submit??6H   $??75??6Y   $??30??6?   ?? 5Q     -+   5        $1 buttonExitIssue??5Q     $??2??66   $??Exit without saving??6F   $	??115??6d   $??30??6?   $??0,0,10,0q?6?   5R     5S     5T     ?? 5V  
   -,   5        $ TabBaseRate??5V     $??BaseRate??6'   # [ 69   .??5W     ?? $??0,1,0,-1q?6   .+?5X     
  --   5        $ BaseRateDataGrid??5X      .   . $
??auto??6p   B ?? Left$B 10??5Y  5   $ VisibleWhenSelected=?6F   
C ?? Top$C 10??6u   $	??400??6?    .  .  $ CellOrRowHeader=?5Z  M   ??5Y     ??  5[     ?? 5\     "  6    .?5]     ?? 
 ? ??
 ? ???6<   ?? 5^     
 ? ??$ 30??67   ?? 5_     
 ? ??
 ?  ??6<   ?? 5`     
 ? ??$ 3q?6A   5a      5d       5e     -.   5        dateFromColumn??$ 	Date From??5e  I   $	 100 6\   & 5f     ?? 5g  "   .R?5h  &     @ 61   ?? *
DateFrom+$# TwoWay=?) . A . $* PropertyChanged=?5i  #   5k       5l     -/   5        valueColumn??$ Value??5l  ?   #. [ 5m  -   $ 60 6V   ! .  " 5l  B   ?? *Value+#, [	 5m  a     5n     -0   5        modifiedByColumn??$ Modified By??5n  f   $
 Auto 6{   ! . " 6G   ?? *
ModifiedBy+6?     5o     -1   5        modifiedOnColumn??$ Modified On??5o  G   $
 Auto 6\   ! . " 5p  -   ?? *
DateFrom+#, [ 5s     ?? 5t     -2   5        $1 buttonCalculateBaseRate??5t     $??Calculate a base rate??6:   $	B 481??6Z   $C 54??6l   $	??160??6|   6?   ?? 5u     $	B 477??6   $C 81??6*   $	??164??6:   .??6F   Proposed base rate6Z   ?? 5v     -3   5        $3 TextProposedBaseRate??5v     $	B 481??68   $	C 112??6J   $	??160??6[   D ?? 
IsReadOnlyD . 6{   ?? 5w     -4   5        $1 buttonAddBaseRate??5w     $??Add a new BaseRate??64   $	B 481??6Q   $	C 338??6c   $	??160??6t   6?   ?? 5x     -5   5        $1 buttonSubmitBaseRate??5x     $??Submit??67   $	B 481??6H   $	C 379??6Z   $	??160??6k   6w   ?? 5y     -6   5        $1 buttonExitBaseRate??5y     $??Exit without saving??65   $	B 481??6S   $	C 421??6e   $	??160??6v   6?   5{     5|     5}     5~     