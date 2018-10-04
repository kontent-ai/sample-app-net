const previewModeToggleClassname = 'preview-mode-switch';
const previewModeCookieName = 'IsPreviewMode';
const previewApiKeyCookieName = 'PreviewApiKey';
const enterPreviewApiKeyFirstMessage = 'Enter your Preview API key';
const enterPreviewApiKeyNextMessage = 'Enter your Delivery Preview API key -- you can find it in Kentico Cloud under Project settings -> API keys';
const enterPreviewApiKeyPromptTitle = 'Preview API key';

const previewModeToggle = document.getElementsByClassName(previewModeToggleClassname).item(0);
const isPreviewModeEnabled = getCookie(previewModeCookieName);
previewModeToggle.checked = isPreviewModeEnabled === 'true' ? true : false;

function trySetPreviewModeValue(newValue, projectId, isFirstTry ) {
    if (newValue) {
        const previewApiKey = getCookie(previewApiKeyCookieName);
        if (previewApiKey === '') {
            enterPreviewApiKey(isFirstTry , projectId);
        } else {
            setPreviewModeEnabledCookie(newValue);
            location.reload();
        }
    } else {
        setPreviewModeEnabledCookie(newValue);
        location.reload();
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
        const deliverRequestUrl = 'https://preview-deliver.kenticocloud.com/' + projectId + '/items/home';
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
    location.reload();
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

function logAttemptToEnterPreviewKey(isFirstAttempt) {
    const label = isFirstAttempt ? 'first' : 'another';
    ga('send', 'event', {
        eventCategory: 'Administrative section',
        eventAction: 'Enter Preview Api key attempt',
        eventLabel: label,
    });
}

function logEnterPreviewKeyResult(isSuccessful) {
    const result = isSuccessful ? 'succesful' : 'not-succesful' 
    ga('send', 'event', {
        eventCategory: 'Administrative section',
        eventAction: 'Enter Preview Api key result',
        eventLabel: result,
    });
}
