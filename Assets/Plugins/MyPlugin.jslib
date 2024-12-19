mergeInto(LibraryManager.library, {
	// Define a function that Unity can call
	onUnityResolutionChange: function (width, height, isFullScreen) {
		// Call the function in the window object (which was defined in index.html)
		if (window.onUnityResolutionChange) {
			window.onUnityResolutionChange(width, height,isFullScreen); // Call the function in index.html
		} else {
			console.error(
				"onUnityResolutionChange function not found in the window object"
			);
		}
	},
	onOpenJotForm: function () {
		if (window.onOpenJotForm) {
			window.onOpenJotForm(); // Call the function in index.html
		} else {
			console.error("onOpenJotForm function not found in the window object");
		}
	},
});
