const previewModeToggleClassname = 'preview-mode-switch';
const previewModeCookieName = 'IsPreviewMode';
const previewApiKeyCookieName = 'PreviewApiKey';
const enterPreviewApiKeyFirstMessage = 'Enter your Preview API key';
const enterPreviewApiKeyNextMessage = 'Enter your Delivery Preview API key -- you can find it in Kentico Kontent under Project settings -> API keys';
const enterPreviewApiKeyPromptTitle = 'Preview API key';
const requestPreviewKeyPromptSearchParam = 'promptPreviewKey';

const previewModeToggle = document.getElementsByClassName(previewModeToggleClassname).item(0);
const isPreviewModeEnabled = getCookie(previewModeCookieName);
previewModeToggle.checked = isPreviewModeEnabled === 'true' ? true : false;

function ensurePreviewModeToggleChecked() {
    previewModeToggle.checked = true;
}

function isRequestedPreviewKeyPrompt() {
    return new URLSearchParams(location.search).get(requestPreviewKeyPromptSearchParam) === 'true';
}

function getSearchStringWithoutPreviewKeyPrompt() {
    const params = new URLSearchParams(location.search);
    params.delete(requestPreviewKeyPromptSearchParam);
    return params.toString();
}

function reloadPage() {
    if (isRequestedPreviewKeyPrompt) {
        location.search = getSearchStringWithoutPreviewKeyPrompt();
    } else {
        location.reload();
    }
}

function getCurrentProjectId() {
    const projectElement = document.getElementById("kc-project-id");
    return projectElement ? projectElement.value : null;
}

window.addEventListener('load',
    function() {
        if (isRequestedPreviewKeyPrompt()) {
            const projectId = getCurrentProjectId();
            if (projectId) {
                ensurePreviewModeToggleChecked();
                trySetPreviewModeValue(true, projectId, true);
            }
        }
    }
);

function trySetPreviewModeValue(newValue, projectId, isFirstTry ) {
    if (newValue) {
        const previewApiKey = getCookie(previewApiKeyCookieName);
        if (previewApiKey === '') {
            enterPreviewApiKey(isFirstTry , projectId);
        } else {
            setPreviewModeEnabledCookie(newValue);
            reloadPage();
        }
    } else {
        setPreviewModeEnabledCookie(newValue);
        reloadPage();
    }
}

function enterPreviewApiKey(isFirstTry , projectId) {
    logAttemptToEnterPreviewKey(isFirstTry);
    const message = isFirstTry  ? enterPreviewApiKeyFirstMessage : enterPreviewApiKeyNextMessage;
    const previewApiKey = prompt(message, enterPreviewApiKeyPromptTitle);
    if (previewApiKey === null) {
        const previewModeToggle = document.getElementsByClassName(previewModeToggleClassname).item(0);
        previewModeToggle.checked = false;
    } else {
        validatePreviewApiKey(previewApiKey, projectId)
        .then(function() {
            processValidPreviewApiKey(previewApiKey);
        })
        .catch(function() {
            processInvalidPreviewApiKey(projectId);
        });
    }
}

function validatePreviewApiKey(previewApikey, projectId) {
    return new Promise(function(resolve, reject) {
        const deliverRequestUrl = 'https://preview-deliver.kontent.ai/' + projectId + '/items/home';
        const xhr = new XMLHttpRequest();
        xhr.open('GET', deliverRequestUrl, true);
        xhr.setRequestHeader("authorization", `Bearer ${previewApikey}`);
        xhr.onload = function() {
            if (this.status >= 200 && this.status < 300) {
                resolve();
            } else {
                reject();
            }
        };
        xhr.onerror = function() {
            reject();
        };
        xhr.send();
    });
}

function processValidPreviewApiKey(previewApiKey) {
    logEnterPreviewKeyResult(true);
    setPreviewModeEnabledCookie(true);
    setPreviewApiKeyCookie(previewApiKey);
    reloadPage();
}

function processInvalidPreviewApiKey(projectId) {
    logEnterPreviewKeyResult(false);
    trySetPreviewModeValue(true, projectId, false);
}

function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) === ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) === 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function setPreviewModeEnabledCookie(isEnabled) {
    document.cookie = `IsPreviewMode=${isEnabled}; max-age=31536000; path=/`;
}

function setPreviewApiKeyCookie(previewApiKey) {
    document.cookie = `PreviewApiKey=${previewApiKey}; max-age=31536000; path=/`;
}

function safeGa(callback, retryCount) {
    if (retryCount === 0) {
        return;
    }

    const ga = window[window['GoogleAnalyticsObject'] || 'ga'];
    if (typeof ga === 'function') {
        callback();
    } else {
        setTimeout(safeGa(callback, retryCount - 1), 200);
    }
}

function logAttemptToEnterPreviewKey(isFirstAttempt) {
    const label = isFirstAttempt ? 'first' : 'another';
    safeGa(() => ga('send', 'event', {
        eventCategory: 'Administrative section',
        eventAction: 'Enter Preview Api key attempt',
        eventLabel: label
    }), 3);
}

function logEnterPreviewKeyResult(isSuccessful) {
    const result = isSuccessful ? 'succesful' : 'not-succesful';
    safeGa(() => ga('send', 'event', {
        eventCategory: 'Administrative section',
        eventAction: 'Enter Preview Api key result',
        eventLabel: result
    }), 3);
}
