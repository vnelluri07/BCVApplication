window.bcvEditor = {
    insertContent: function (editorId, html) {
        var editor = tinymce.get(editorId);
        if (!editor) return false;

        editor.undoManager.transact(function () {
            var node = editor.selection.getNode();
            var wrapper = editor.dom.create('div');
            wrapper.innerHTML = html;

            while (wrapper.firstChild) {
                node.parentNode.insertBefore(wrapper.firstChild, node.nextSibling);
            }
        });

        editor.nodeChanged();
        return true;
    }
};

/* ── Card re-arrange controls ── */
(function () {
    function removeToolbar(editor) {
        var old = editor.getBody().querySelectorAll('.bcv-card-toolbar');
        old.forEach(function (t) { t.remove(); });
    }

    function handleCardClick(editor, target) {
        var card = target.closest('.link-preview-card, .video-embed');

        // Clicked a toolbar button
        var btn = target.closest('[data-card-action]');
        if (btn) {
            var actionCard = btn.closest('.link-preview-card, .video-embed');
            if (!actionCard) return;
            var action = btn.getAttribute('data-card-action');

            editor.undoManager.transact(function () {
                if (action === 'up') {
                    var prev = actionCard.previousElementSibling;
                    if (prev) actionCard.parentNode.insertBefore(actionCard, prev);
                } else if (action === 'down') {
                    var next = actionCard.nextElementSibling;
                    if (next) actionCard.parentNode.insertBefore(next, actionCard);
                } else if (action === 'left') {
                    actionCard.style.marginLeft = '0';
                    actionCard.style.marginRight = 'auto';
                } else if (action === 'center') {
                    actionCard.style.marginLeft = 'auto';
                    actionCard.style.marginRight = 'auto';
                } else if (action === 'right') {
                    actionCard.style.marginLeft = 'auto';
                    actionCard.style.marginRight = '0';
                } else if (action === 'delete') {
                    actionCard.remove();
                }
            });
            removeToolbar(editor);
            editor.nodeChanged();
            return;
        }

        // Clicked outside a card — dismiss
        if (!card) {
            removeToolbar(editor);
            return;
        }

        // Clicked a card — show toolbar
        removeToolbar(editor);
        var toolbar = editor.dom.create('div', {
            'class': 'bcv-card-toolbar',
            'contentEditable': 'false'
        });
        toolbar.innerHTML =
            '<button data-card-action="up" title="Move up">&#8593;</button>' +
            '<button data-card-action="down" title="Move down">&#8595;</button>' +
            '<button data-card-action="left" title="Align left">&#9664;</button>' +
            '<button data-card-action="center" title="Center">&#9632;</button>' +
            '<button data-card-action="right" title="Align right">&#9654;</button>' +
            '<button data-card-action="delete" title="Remove">&#10005;</button>';

        card.insertBefore(toolbar, card.firstChild);
    }

    function attachControls(editor) {
        // Use TinyMCE's event system — native DOM events don't reach noneditable elements
        editor.on('click', function (e) {
            handleCardClick(editor, e.target);
        });
    }

    if (typeof tinymce !== 'undefined') {
        tinymce.on('AddEditor', function (e) {
            e.editor.on('init', function () {
                attachControls(e.editor);
            });
        });
    }
})();
