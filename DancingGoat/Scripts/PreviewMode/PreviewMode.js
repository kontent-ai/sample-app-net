const previewModeToggleClassname = 'preview-mode-switch';
const previewModeCookieName = 'IsPreviewMode';
const previewApiKeyCookieName = 'PreviewApiKey';
const enterApiKeyFirstMessage = 'Enter your Preview API key';
const enterApiKeyNextMessage = 'Enter your Delivery Preview API key -- you can find it in Kentico Cloud under Project settings -> API keys';
const enterApiKeyPromptTitle = 'Preview API key';

const previewModeToggles = document.getElementsByClassName(previewModeToggleClassname);
const isPreviewModeEnabled = getCookie(previewModeCookieName);
for (toggle of previewModeToggles) {
    toggle.checked = isPreviewModeEnabled === 'true' ? true : false;
}

function togglePreviewMode(isChecked, projectId, isFirstEnterOfApiKey) {
    if (isChecked) {
        const apiKey = getCookie(previewApiKeyCookieName);
        if (apiKey === '') {
            enterApiKey(isFirstEnterOfApiKey, projectId);
        } else {
            setPreviewModeEnabledCookie(isChecked);
            location.reload();
        }
    } else {
        setPreviewModeEnabledCookie(isChecked);
        location.reload();
    }
}

function enterApiKey(isFirstEnterOfApiKey, projectId) {
    logAttemptToEnterPreviewKey(isFirstEnterOfApiKey);
    const message = isFirstEnterOfApiKey ? enterApiKeyFirstMessage : enterApiKeyNextMessage;
    const apiKey = prompt(message, enterApiKeyPromptTitle);
    if (apiKey === null) {
        const previewModeToggles = document.getElementsByClassName(previewModeToggleClassname);
        for (toggle of previewModeToggles) {
            toggle.checked = false
        }
    }

    if (apiKey !== null) {
        validateApiKey(apiKey, projectId);
    }
}

function validateApiKey(apiKey, projectId) {
    const deliverRequestUrl = 'https://preview-deliver.kenticocloud.com/' + projectId + '/items/home';
    const xhr = new XMLHttpRequest();
    xhr.open('GET', deliverRequestUrl, true);
    xhr.setRequestHeader("authorization", `Bearer ${apiKey}`);
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
            logEnterPreviewKeyResult(true);
            setPreviewModeEnabledCookie(true);
            setPreviewApiKeyCookie(apiKey);
            location.reload();
        }

        if (xhr.readyState === 4 && xhr.status === 401) {
            logEnterPreviewKeyResult(false);
            togglePreviewMode(true, projectId, false);
        }
    };
    xhr.send();
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

function setPreviewApiKeyCookie(apiKey) {
    document.cookie = `PreviewApiKey=${apiKey}; max-age=31536000; path=/`;
}

function logAttemptToEnterPreviewKey(isFirstAttempt) {
    const label = isFirstAttempt ? 'first' : 'another';
    ga('send', 'event', {
        eventCategory: 'Administrative section',
        eventAction: 'Enter Api key attempt',
        eventLabel: label,
    });
}

function logEnterPreviewKeyResult(isSuccessful) {
    const result = isSuccessful ? 'succesful' : 'not-succesful' 
    ga('send', 'event', {
        eventCategory: 'Administrative section',
        eventAction: 'Enter Api key result',
        eventLabel: result,
    });
}
