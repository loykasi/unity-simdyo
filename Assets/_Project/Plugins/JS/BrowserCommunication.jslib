mergeInto(LibraryManager.library, {
    OnGameLoaded: function() {
        window.parent.postMessage({ type: "gameLoaded" }, "*");
    }
})