
var editorDiv = document.getElementById('editor');
if (editorDiv) {
    const quill = new Quill('#editor', {
        placeholder: 'Compose an epic...',
        theme: 'bubble',
    });
    quill.enable(false);
}


const FontAttributor = Quill.import('attributors/class/font');
FontAttributor.whitelist = [
    'lora'
];
Quill.register(FontAttributor, true);

const toolbarOptions = [
    ['bold', 'italic', 'underline', 'strike'],        // toggled buttons
    ['blockquote', 'code-block'],
    ['link', 'image', 'video', 'formula'],

    [{ 'header': 1 }, { 'header': 2 }],               // custom button values
    [{ 'list': 'ordered' }, { 'list': 'bullet' }, { 'list': 'check' }],
    [{ 'script': 'sub' }, { 'script': 'super' }],      // superscript/subscript
    [{ 'indent': '-1' }, { 'indent': '+1' }],          // outdent/indent
    [{ 'direction': 'rtl' }],                         // text direction

    [{ 'size': ['small', false, 'large', 'huge'] }],  // custom dropdown
    [{ 'header': [1, 2, 3, 4, 5, 6, false] }],

    [{ 'color': [] }, { 'background': [] }],          // dropdown with defaults from theme
    [{ 'font': [] }],
    [{ 'align': [] }],

    ['clean']                                         // remove formatting button
];


var editorDiv2 = document.getElementById('editortest');
if (editorDiv2) {
    const quilleditor = new Quill('#editortest', {
        theme: 'snow',
        modules: {
            syntax: true,
            toolbar: toolbarOptions,
            imageResize: {
                displaySize: true
            }
        }
    });
    function getAndDisplayHTML() {
        var htmlContent = quilleditor.root.innerHTML;
        document.getElementById('htmlOutput').innerText = htmlContent;
    }

    let inputElement = document.getElementById('hiddeninput')
    quilleditor.on('text-change', function () {
        // sets the value of the hidden input to
        // the editor content in Delta format
        inputElement.value = quilleditor.root.innerHTML;

        // you can alternatively use
        // inputElement.value = quill.root.innerHTML
        // if you want the data as HTML
    });
}

var input = document.getElementById('Becomings')
if (input) {
    var tagify = new Tagify(input, {
        dropdown: {
            enabled: 0
        }
    })
    var selectedTag = input.getAttribute('data-tags');

    input.value = '';
    // Convert the string of tags into an array
    var tagsArray = selectedTag.split(',');

    tagsArray.forEach(function (tag) {
        // Get the value of each tag
        tagify.addTags([tag.trim()]);
    });
}



function copyFunction() {
    // Get the text field
    var copyText = document.getElementById("fileurl");

    var selectedTag = copyText.getAttribute('data-tags'); // For mobile devices

    // Copy the text inside the text field
    navigator.clipboard.writeText(selectedTag);

}