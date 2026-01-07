mergeInto(LibraryManager.library, {
    OnGameLoaded: function() {
        window.parent.postMessage({ type: "gameLoaded" }, "*");
    }

    OnGameRestarted: function() {
        window.parent.postMessage({ type: "gameRestarted" }, "*");
    }

    OnGamePaused: function() {
        window.parent.postMessage({ type: "gamePaused" }, "*");
    }

    OnGameResumed: function() {
        window.parent.postMessage({ type: "gameResumed" }, "*");
    }
})