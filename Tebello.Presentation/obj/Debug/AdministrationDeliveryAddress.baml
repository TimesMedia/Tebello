   M S B A M L   `   `   `  ????D  @Subs.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null8*clr-namespace:Subs.Data;assembly=Subs.Data	Subs.Data  5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R8clr-namespace:System.ComponentModel;assembly=WindowsBaseSystem.ComponentModel 6A    Tebello.Presentation5   /Subs.Presentation.AdministrationDeliveryAddress   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   ;Data*clr-namespace:Subs.Data;assembly=Subs.Data     5   	   SComponentModel8clr-namespace:System.ComponentModel;assembly=WindowsBase     6A   -   5          ? Title$%  Delivery address administration??5   	   $	??749??61   $
??1250??6>   $
??1250??6N    ? WindowState$ Normal=?6r    ? WindowStartupLocation$ Manual=?6?    ? 	Resources 5      "   Subs.Data.DeliveryAddressDoc   deliveryAddressDoc  deliveryAddressViewSource  countryViewSource ?? Source ?? Source  provinceViewSource  provinceCityViewSource ?? SortDescriptions+  %System.ComponentModel.SortDescription   	Direction   PropertyName   provinceCitySuburbViewSource & "provinceCitySuburbStreetViewSource  
AlignRight	 ?? 
TargetType
 ?? Property ?? Value*  $System.Windows.Controls.DataGridCell ?? Triggers P? Property P? Value&   System.Windows.Controls.DataGrid ?? RelativeSource??%6  &	        &	      &	 &     7   &	 S     7  &	 ?     7  &	     7  &	 C    7  &	 y    '?? ?    '      '  ?      5   
   6"   ?? 5   
   6   ?? 5
   
    69   ?? *	Country+8   ?? 5   
    6:   ?? *FK_Province_Country+8   ?? 5   
    6>   ?? *FK_City_Province+8    5        5      $ 	Ascending=?61   $ CityName??6G   5      ?? 5   
    6D   ?? *FK_Suburb_City+8   ?? 5   
    6J   ?? *FK_Street_Suburb+8   ?? 5   
   "	 ?6#   .?5      ?? 
 ? ??$ Right=?64   5      ?? 5   
   "	 ??6   .?5      ?? 
 ? ??
 ? ???6+   5      ?? 5   
   "	  6    5      P? 5        ?@ 
IsSelected . . ?5!      ?? 
 ? ??
 ? ???63   ?? 5"      
 ? ??
 ?   ?63   5#      5%      ?? 5'   
   "	  6   .?5(      ?? 
 ? ??$ 	10,10,0,0q?6'    5*      P? 5+       ?@=?HasError . . ?5,      ?? 
 ?@?ToolTip 5.      ?? *%#(Validation.Errors)[0].ErrorContent+ ?? *Self+5/      51      53      .??54      ?? -   5         ??Name$ 
TabControl??54      .@?55   
   ?? $??Non verified addresses??6   .??56        -   5          Name$ deliveryAddressDataGrid??56      $??1??68      AutoGenerateColumns .     EnableRowVirtualization .    RowDetailsVisibilityMode$ VisibleWhenSelected=?6?   $
??Auto??57   !   $   HorizontalScrollBarVisibility$ Visible=?61   $
??1080??6Y      SelectionUnit$ CellOrRowHeader=?6f      	RowHeight$ 30Q?6?      
IsReadOnly . $ ?? IsSynchronizedWithCurrentItem$
 TrueY?58   3   $??10q?59   !    ? DataContext# [ 6-   ??6f   ??    RowHeaderStyle 5:      ?? 5;      :  4System.Windows.Controls.Primitives.DataGridRowHeader"	  6   .?5<      ?? 
 ? ??
 ? ???68   ?? 5=      
 ? ??$ 30??63   ?? 5>      
 ? ??
 ?  ??68   ?? 5?      
 ? ??$ 3q?6=   5@         Columns 5B      0  *System.Windows.Controls.DataGridTextColumn  5C      -   5        deliveryAddressIdColumn??,  &System.Windows.Controls.DataGridColumn   Header$ Delivery Address Id??5C   p      
IsReadOnly .    	CellStyle# [ 5D   1       Width5  /System.Windows.Controls.DataGridLengthConverter$
  Auto 6Y   1	  +System.Windows.Controls.DataGridBoundColumn! 	  Binding! 5C   J   ?? *DeliveryAddressId+5D   Y     5E      -   5        countryStringColumn??$ Country??5E   f   $
  Auto 6w   ! 6F   ?? *CountryName+6w     5F      -   5        provinceColumn??$ Province??5F   k   $
  Auto 6}   ! 6A   ?? *
Province+" ?? Mode$" OneWay=?6}     5G      -   5        
cityColumn??$
 City??5G   V   $
  Auto 6d   ! 6=   ?? *City+6d     5H      -   5        suburbColumn??$ Suburb??5H   Z   $
  Auto 6j   ! 6?   ?? *Suburb+6j     5I      -	   5        streetColumn??$ Street??5I   Z   $
  Auto 6j   ! 6?   ?? *Street+6j     5J      -
   5        streetExtensionColumn1??$ Street Extension??5J   m   $
  Auto 6?   ! 6I   ?? *StreetExtension+6?     5K      -   5        streetSuffixColumn1??$ Street Suffix??5K   g   $
  Auto 6~   ! 6F   ?? *StreetSuffix+6~     5L      -   5        streetStreetIdColumn1??$ StreetId??5L   e   $
  Auto 6w   ! 6H   ?? *
StreetId+6w     5M      -   5        streetNoColumn??$ 	Street No??5M   ^   $
  Auto 6q   # [ 5N   1   ! 5M   A   ?? *
StreetNo+5N   1     5O      -   5        buildingColumn??$ Building??5O   ^   $
  Auto 6p   ! 6A   ?? *
Building+6p     5P      -   5        floorColumn??$ Floor??5P   Z   $
  Auto 6i   ! 6>   ?? *	FloorNo+6i     5Q      -   5        
roomColumn??$
 Room??5Q   V   $
  Auto 6d   # [ 5R   1   ! 5Q   =   ?? *Room+5R   1     5S      -   5        postCodeColumn??$ 	Post Code??5S   ^   $
  Auto 6q   # [ 5T   1   ! 5S   A   ?? *
PostCode+5T   1     5U      -   5        phoneNumberColumn??$ Phone Number??5U   d   $
  Auto 6z   ! 6D   ?? *PhoneNumber+6z   # ? ContextMenu# 5W      ?? 5X      .@?5Y      m? -   5        $ m?Name$$ contextSelect??5Y   #   $??Select??6:   6J   5Z      5\      5]      ?? 5_   
   $??Address Template??6   .??5`      ? -   5        % ?Name$% AddressTree??5`      $??Stretch=?6(   $
??auto??6F   & ? ColumnDefinitions& 5a      ?? 5b      $	??46* ?6'   ?? 5c      $	??34* ?6'   ?? 5d      $
??194* ?6'   ?? 5e      $
??275* ?6'   ?? 5f      $
??229* ?6'   ?? 5g      $
??458* ?6'   ' ? RowDefinitions' 5i      ?? 5j      $??35 ?6$   ?? 5k      $??35 ?6$   ?? 5l      $??* ?6$   ?? 5m      $??* ?6$   ?? 5n      $??40 ?6$   .+?5q      ?? -   5        ( ??Name$( textCountry??5q      ) ?? 
IsReadOnly) . $??0??6B   $??0??6O   $??3??6_   ?? 5r      -   5        $( textProvince??5r      ) . $??1??6C   $??0??6P   $??3??6`   ?? 5s      -   5        $( textCity??5s      ) . $??0??6@   $??3??6M   $??10,0,0,0q?6]   ?? 5t      -   5        $( 
textSuburb??5t      ) . $??0??6A   $??4??6N   $??10,0,0,0q?6^   ?? 5u      * ?? Orientation$* 
Horizontal=?6   $??0??67   $??5??6D   .+?5v      ?? -   5        $( 
textStreet??5v      ) . $??10,0,0,0q?6E   ??6W   ?? *ActualWidth++ ?? ElementName$+ streetNameColumn???? 5x      -   5        $( textExtension??5x      ) . ??6H   ?? *ActualWidth+$+ streetExtensionColumn???? 5z      -   5        $( 
textSuffix??5z      ) . ??6E   ?? *ActualWidth+$+ streetSuffixColumn??5{        5}      -   5        $ countryDataGrid??5}       .   .  $??2??6p   $??0??6}   $ VisibleWhenSelected=?5~   Y   $
??auto??5       . $??3??66   $
 TrueY?6J   ??5~      ?? # [  5?        5?      -   5        countryNameColumn??$ Country Name??5?   h   $
  auto 6~   ! 6H   ?? *CountryName+6~     5?      -   5        countryIdColumn??$ 	CountryId??5?   f   $
  auto 6y    . ! 6F   ?? *CountryName+6?   5?        5?      -   5        $ Province_DataGrid??5?      $ VisibleWhenSelected=?66   $??0,0,0,0q?6e    .  .  $
??auto??5?   R   $??3??6_   $??3??6l   ??5?      ?? # [  5?        5?      -    5        provinceNameColumn??$
  Auto 5?   I   $ Province Name??6V   ! 6m   ?? *ProvinceName+  5?      $
  Auto 6-   $ 	CountryId??6:   ! 6M   ?? *	CountryId+  5?      -!   5        provinceIdColumn??$
  Auto 5?   G   $ 
ProvinceId??6T    . ! 6h   ?? *
ProvinceId+6?   5?        5?      -"   5        $ City_DataGrid??5?       .   . $
??auto??6n   $ VisibleWhenSelected=?5?   ^   $??2??5?      $??3??6$   $??2??64   ??5?      ?? # [  5?        5?      -#   5        cityNameColumn??$ 	City Name??5?   b   $
  auto 6u   ! 6E   ?? *
CityName+6u     5?      -$   5        cityIdColumn??$ CityId??5?   ^   $  SizeToHeader 6n    . ! 6C   ?? *CityId+6?     5?      $ 
ProvinceId??6M   $  SizeToHeader 6a   ! 6.   ?? *
ProvinceId+6a   # 5?      ?? 5?      .@?5?      m? -%   5        $??Create list of customers??5?   '   6I   5?      5?        5?      -&   5        $ Suburb_DataGrid??5?       .   . $ VisibleWhenSelected=?5?      $??2??6F   $??4??6S   $
??auto??6c   $??2??6p   ??5?      ?? # [  5?        5?      -'   5        suburbNameColumn??$ Suburb Name??5?   f   $
  auto 6{   ! 6G   ?? *
SuburbName+6{     5?      -(   5        suburbIdColumn??$ SuburbId??5?   b   $  SizeToHeader 6t    . ! 6E   ?? *
SuburbId+6?     5?      $ CityId??6I   $  SizeToHeader 6Y   ! 6.   ?? *CityId+6Y   # 5?      ?? 5?      .@?5?      m? -)   5        $??Create list of customers??5?   '   6I   5?      5?        5?      -*   5        $ Street_DataGrid??5?       .   . $
??auto??6o   $??2??6|   $??2??6?   $ VisibleWhenSelected=?5?   j   $??5??6?   ??6   ?? # [  5?        5?      -+   5        streetNameColumn??$ Street Name??5?   f   $
  auto 6{   ! 6G   ?? *
StreetName+6{     5?      -,   5        streetExtensionColumn??$ Street Extension??5?   p   $
  auto 6?   ! 6L   ?? *StreetExtension+6?     5?      --   5        streetSuffixColumn??$ Street Suffix??5?   j   $
  auto 6?   ! 6I   ?? *StreetSuffix+6?     5?      $ SuburbId??6J   $
  auto 6\   ! 6-   ?? *
SuburbId+6\     5?      $ StreetId??6K   $
  auto 6]    . ! 6.   ?? *
StreetId+6j   5?      ?? 5?      -.   5        $??Submit Country??5?      $??4??62   $	??101??6?   $
??Left=?6K   $??3??6~   ?? 5?      -/   5        $??Submit Province??5?      $??4??63   $??97??6@   $??Right=?6L   $??2??6?   ?? 5?      -0   5        $??Submit City??5?      $??4??6/   $??3??6<   $??75??6L   $??100,0q?6j   ?? 5?      -1   5        $??Submit Suburb??5?      $??4??62   $??4??6?   $	??100??6d   ?? 5?      $* 
Horizontal=?6   $??4??66   $??5??6C   .+?5?      ?? -2   5        $??Submit Street??5?      $??83??66   $??12,0,0,0q?6V   ?? 5?      -3   5        $??Standardise address??5?      $	??135??6<   $??	180,0,0,0q?6_   5?      5?      5?      5?      5?      