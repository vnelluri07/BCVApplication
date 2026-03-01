window.bcvOffline = {
    isOnline: () => navigator.onLine,

    registerOnlineListener: (dotNetRef, methodName) => {
        const handler = () => dotNetRef.invokeMethodAsync(methodName);
        window.addEventListener('online', handler);
        return handler;
    },

    saveDrafts: (json) => localStorage.setItem('bcv_offline_drafts', json),

    loadDrafts: () => localStorage.getItem('bcv_offline_drafts'),

    clearDrafts: () => localStorage.removeItem('bcv_offline_drafts')
};
