// TabVisibility.js

// Function to detect tab visibility changes
function detectTabVisibilityChanges() {
    var hidden, visibilityChange;

    if (typeof document.hidden !== "undefined") {
        hidden = "hidden";
        visibilityChange = "visibilitychange";
    } else if (typeof document.msHidden !== "undefined") {
        hidden = "msHidden";
        visibilityChange = "msvisibilitychange";
    } else if (typeof document.webkitHidden !== "undefined") {
        hidden = "webkitHidden";
        visibilityChange = "webkitvisibilitychange";
    }

    if (typeof document.addEventListener === "undefined" || hidden === undefined) {
        // Browser doesn't support visibility API
        console.log("Tab visibility detection not supported in this browser.");
    } else {
        // Listen for visibility change events
        document.addEventListener(visibilityChange, handleVisibilityChange, false);
    }
}

// Function to handle tab visibility change
function handleVisibilityChange() {
    if (document.hidden) {
        // Tab is not active
        UnityLoader.SystemInfo.OnApplicationFocus(false); // Notify Unity that tab is not active
    } else {
        // Tab is active
        UnityLoader.SystemInfo.OnApplicationFocus(true); // Notify Unity that tab is active
    }
}

detectTabVisibilityChanges(); // Start listening for tab visibility changes
