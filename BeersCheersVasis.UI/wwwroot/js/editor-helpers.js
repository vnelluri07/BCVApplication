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

        // Tab key inserts spaces instead of moving focus
        editor.on('keydown', function (e) {
            if (e.keyCode === 9) {
                e.preventDefault();
                editor.execCommand('mceInsertContent', false, '&emsp;');
            }
        });

        // Autocorrect common typos on space/punctuation
        var fixes = {
            'teh':'the','dont':'don\'t','doesnt':'doesn\'t','didnt':'didn\'t',
            'cant':'can\'t','wont':'won\'t','im':'I\'m','ive':'I\'ve',
            'youre':'you\'re','theyre':'they\'re','thats':'that\'s',
            'isnt':'isn\'t','wasnt':'wasn\'t','werent':'weren\'t',
            'couldnt':'couldn\'t','shouldnt':'shouldn\'t','wouldnt':'wouldn\'t',
            'hes':'he\'s','shes':'she\'s','its':'it\'s','lets':'let\'s',
            'whats':'what\'s','whos':'who\'s','theres':'there\'s',
            'arent':'aren\'t','hasnt':'hasn\'t','havent':'haven\'t',
            'recieve':'receive','occured':'occurred','seperate':'separate',
            'definately':'definitely','accomodate':'accommodate',
            'occurence':'occurrence','neccessary':'necessary',
            'wierd':'weird','untill':'until','becuase':'because',
            'tho':'though','thru':'through','alot':'a lot',
            'i':'I'
        };
        editor.on('keydown', function (e) {
            if (e.keyCode !== 32 && e.keyCode !== 190 && e.keyCode !== 188) return;
            var rng = editor.selection.getRng();
            var textNode = rng.startContainer;
            if (textNode.nodeType !== 3) return;
            var text = textNode.textContent.substring(0, rng.startOffset);
            var m = text.match(/(\S+)$/);
            if (!m) return;
            var word = m[1];
            var fixed = fixes[word.toLowerCase()];
            if (!fixed) return;
            // preserve leading capital
            if (word[0] === word[0].toUpperCase() && word !== 'i')
                fixed = fixed[0].toUpperCase() + fixed.slice(1);
            var start = rng.startOffset - word.length;
            textNode.textContent = textNode.textContent.substring(0, start) + fixed + textNode.textContent.substring(rng.startOffset);
            rng.setStart(textNode, start + fixed.length);
            rng.setEnd(textNode, start + fixed.length);
            editor.selection.setRng(rng);
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
