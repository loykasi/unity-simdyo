mergeInto(LibraryManager.library, {
    LoadFile: function(gameObjectNamePtr, methodNamePtr, filterPtr, multiselect) {
        gameObjectName = UTF8ToString(gameObjectNamePtr);
        methodName = UTF8ToString(methodNamePtr);
        filter = UTF8ToString(filterPtr);

        let input = document.getElementById(gameObjectName);
        if (input) {
            document.body.removeChild(input);
        }

        input = document.createElement('input');
        input.setAttribute('id', gameObjectName);
        input.setAttribute('type', 'file');
        input.setAttribute('style','display:none;');
        input.setAttribute('style','visibility:hidden;');

        if (multiselect) {
            input.setAttribute('multiple', '');
        }
        if (filter) {
            input.setAttribute('accept', filter);
        }

        input.onclick = function (event) {
            this.value = null;
        };
        input.onchange = function (event) {
            const files = Array.from(event.target.files);
            const urls = files.map(file => URL.createObjectURL(file));
            const json = JSON.stringify(urls);
            SendMessage(gameObjectName, methodName, json);
        }
        
        document.body.appendChild(input);
        input.click();
        document.body.removeChild(input);
    },

    SaveFile: function(fileNamePtr, byteArray, byteArraySize) {
        fileName = UTF8ToString(fileNamePtr);

        var bytes = new Uint8Array(byteArraySize);
        for (var i = 0; i < byteArraySize; i++) {
            bytes[i] = HEAPU8[byteArray + i];
        }

        const blob = new Blob([bytes], { type: 'application/zip' });
        const url = URL.createObjectURL(blob);

        const downloader = document.createElement('a');
        downloader.href = url;
        downloader.download = fileName;

        document.body.appendChild(downloader);
        downloader.click();
        document.body.removeChild(downloader);
    }
})