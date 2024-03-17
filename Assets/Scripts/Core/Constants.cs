namespace Core
{
	public static class Constants
	{
		public const string ConfigsPath = "Configs/";

		public const string CardsConfigPath = "CardsConfig";
		
		public const string NumsCards = "The number of cards: ";
		public const string TextCountCards = "TextCountCards";
		public const string ErrorText = "ErrorText";
		
		public const string ShowKey = "Show";
		public const string HideKey = "Hide";
		
		public const string PanelMainView = "PanelMainView";

		public const string PopupAnim = "Popup";
		public const string IsAnimatedCard = "IsAnimatedCard";
		public const string IsRefreshAllCards = "IsRefreshAllCards";
		public const string InputCountCard = "InputCountCard";
		
		public const string ActiveCards = "ActiveCards";
		public const string InactiveCards = "InactiveCards";
		
		public const string DropdownColors = "DropdownColors";
		
		public const string BtnClosePopup = "PanelView";
		public const string BtnOkPopup = "BtnOkPopup";
		
		public const string URL = "https://65f1f7bf034bdbecc7642352.mockapi.io/cards";
		
		public enum UIType
		{
			None,
			Create,
			Delete,
			Update,
			Refresh
		}
	}
}
