   M S B A M L   `   `   `  ????O  KTebello.Presentation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Subs.PresentationSubs.Presentation  5   	   D @Subs.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null8*clr-namespace:Subs.Data;assembly=Subs.Data	Subs.Data 5   	   .    (Subs.Presentation.AdministrationPostCode   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:Subs.Presentation     5   	   ;Data*clr-namespace:Subs.Data;assembly=Subs.Data     5   	   -   5        $??PostCodeQueryWindow??5
   	     ? Title$  PostCode Administration??5	   	   $	??400??6)   $??934.942??69    ? WindowStartupLocation$ CenterScreen=?5
   $    ? 	Resources 5        Subs.Data.PostCodeDoc   postCodeDoc  postCode_LinearViewSource ?? Source ?? Source*  $System.Windows.Controls.DataGridCell ?? 
TargetType ?? Triggers P? Property P? Value	 ?? Property
 ?? Value  
AlignRight??%m  &	        &	      7   '  H     &	 ?       5   
   6   ?? 5   
    6A   ?? *PostCode_Linear+8   ?? 5   
   "  6    5      P? 5       ?@ 
IsSelected . . ?5      ?? 	 ? ??

 ? ???63   ?? 5      	 ? ??

 ?  ??63   5      5      ?? 5   
   " ?6#   .?5      ?? 	 ? ??$
 Right=?64   5      5      .??5      ? -   5         ?Name$ GridLinearView??5       ? ColumnDefinitions 5   
   ?? 5      $	??400 ?6   ?? 5      $??* ?6   .+?5       ?? $??10,10,10,10q?6   .+?5!      ?? $??Search string??6    ?? Left$ 10??60   
 ?? Top$ 10??6A   ?? 5"      -   5        $??
textSearch??5"      $??25??6,   $ 10??68    ?? TextWrapping$
 Wrap=?6I   $ 37??6^   $	??150??6n    ?? 	MaxLength$ 50??6z    ?? CharacterCasing$ Upper=?6?   ?? 5#      -   5        $??buttonSearchSuburb??5#      $??Search Suburb??63   $	 165??6K   $ 37??6]   $	??105??6m   $??24??6z   6?   ?? 5$      -   5        $??buttonSearchCity??5$      $??Search City??61   $	 165??6G   $ 66??6Y   $	??105??6i   $??24??6v   6?   ?? 5%      -   5        $??buttonSearchCode??5%      $??Search Code??61   $	 165??6G   $ 95??6Y   $	??105??6i   $??24??6v   6?   ?? 5&      -   5        $??buttonSelectSource??5&      $??Mark selection as source??63   $ 17??6V   $	 166??6g   $	??143??6x   $??24??6?   6?   ?? 5'      -   5        $??
textSource??5'      $??25??6,   $	 188??68   $
 Wrap=?6J   $	 165??6_   $??82??6p   $ 50??6{    ?? TextAlignment$ Right=?6?    ?? 
IsReadOnly . 6?   ?? 5(      -	   5        $??buttonSelectTarget??5(      $??Mark selection as target??63   $ 17??6V   $	 211??6g   $	??143??6x   $??24??6?   6?   ?? 5)      -
   5        $??
textTarget??5)      $??25??6,   $	 188??68   $
 Wrap=?6J   $	 210??6_   $??82??6p   $ 50??6{   $ Right=?6?    . 6?   ?? 5*      -   5        $??buttonConsolidate??5*      $??Consolidate??62   $ 17??6H   $	 257??6Y   $	??253??6j   $??24??6w   6?   ?? 5+      -   5        $??buttonSubmit??5+      $)??#Submit changes to AddressLine3 or 4??6-   $ 17??6[   $	 310??6l   $	??253??6}   $??24??6?   6?   5,      &   System.Windows.Controls.DataGrid  5.      -   5          Name$ postCode_LinearDataGrid??5.         RowDetailsVisibilityMode$ VisibleWhenSelected=?68   $
??Auto??6g   $??
0,10,10,10q?6u      EnableRowVirtualization .    AutoGenerateColumns .  $??1??5/   R   $
??auto??6b      SelectionMode$ Single=?6o      SelectionUnit$ FullRow=?6?   $??Stretch=?6?   ??5.   ?   ?? # [    Columns 50      0  *System.Windows.Controls.DataGridTextColumn  51      -   5        
typeColumn??,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 51   =      Header$
 Type??6J      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6X   ?? *Type+6q     52      -   5        addressLine3Column??$
 Auto 52   E   $ Address Line 3??6R    .   6j   ?? *AddressLine3+   EditingElementStyle 53      ?? 54      " ??6$   .?55   "   ?? 	 ?@??CharacterCasing$
 Upper=?6D   56      58        59      -   5        addressLine4Column??$
 Auto 59   E   $ Address Line 4??6R    .   6j   ?? *AddressLine4+ 5:      ?? 5;      " ??6$   .?5<   "   ?? 	 ?@??CharacterCasing$
 Upper=?6D   5=      5?        5A      -   5        
codeColumn??$
 Auto 5A   =   $
 Code??6J    .  6X   ?? *Code+6q     5B      -   5        codeIdColumn??$ SizeToHeader 5B   ?    . $ Code Id??6f       
Visibility$  Hidden=?6?    6w   ?? *CodeId+6?     5C      -   5        addressLine4IdColumn??$ SizeToHeader 5C   G    . $ Address Line 4 Id??6n   $  Hidden=?6?    6?   ?? *AddressLine4Id+6?     5D      -   5        addressLine3IdColumn??$ SizeToHeader 5D   G    . $ 
PostCodeId??6n   $  Visible=?6?   !   	CellStyle#! [ 6?    6?   ?? *AddressLine3Id+6?   5F      5G      5H      