
/**
 * uploaded image ids page lifecycle variable
 * @type {Set<string>}
 */
export const uploadedImageIds = new Set();

/**
 * Create toast editor instance
 * Add Image Button Hook makes POST /Admin/Blogs/UploadImage upload image to server,
 * then respond { url, altText } json data
 *
 * @param {string} mdSelector markdown editor selector
 * @param {HTMLTextAreaElement} textarea backend textarea
 * @returns {toastui.Editor}
 */
export function createToastEditor(mdSelector, textarea) {
  const editor = new toastui.Editor({
    el: document.querySelector(mdSelector),
    height: '300px',
    initialEditType: 'markdown',
    previewStyle: 'vertical',
    initialValue: textarea.value ?? '',
    hooks: {
      addImageBlobHook: async (blob, callback) => {
        const token = document.querySelector('input[name="__RequestVerificationToken"]').value;
        
        const formData = new FormData();
        formData.append('image', blob);

        const response = await fetch("/Admin/Blogs/UploadImage", {
          method: 'POST',
          headers: {
            'RequestVerificationToken': token
          },
          body: formData
        });

        if (!response.ok) {
          console.error('Image upload failed:', response.statusText);
          alert('Image upload failed. Please try again.');
          return;
        }

        const result = await response.json();
        
        // save image id
        uploadedImageIds.add(result.id);
        
        // insert markdown contents
        callback(result.url, result.altText ?? blob.name);
      }
    }
  });

  return editor;
}

/**
 * create input has value id
 * @param {string} id uploaded image id
 * @param {string} prefix Razor Pages binding model name
 * @return {HTMLInputElement}
 */
export function createImageIdInput(id, prefix = undefined) {
  const input = document.createElement('input');
  input.type = 'hidden';
  if (prefix) {
    input.name = `${prefix}.ImageIds`;
  } else {
    input.name = 'ImageIds';  
  }
  
  input.value = id;
  return input;
}