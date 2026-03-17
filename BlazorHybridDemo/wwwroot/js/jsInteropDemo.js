// Blazor Hybrid JS Interop Demo
// This file demonstrates JavaScript functions callable from C#

window.blazorDemo = {
    /**
     * Shows a temporary message on the page
     * @param {string} message - The message to display
     * @returns {string} Confirmation message
     */
    showMessage: function(message) {
        const div = document.createElement('div');
        div.className = 'alert alert-primary';
        div.textContent = 'JavaScript says: ' + message;
        document.body.appendChild(div);
        setTimeout(() => div.remove(), 3000);
        return 'Message shown: ' + message;
    },

    /**
     * Changes the content and background color of a DOM element
     * @param {string} elementId - The ID of the element to modify
     * @param {string} newContent - The new HTML content
     */
    changeDom: function(elementId, newContent) {
        const element = document.getElementById(elementId);
        if (element) {
            element.innerHTML = newContent;
            element.style.background = '#' + Math.floor(Math.random()*16777215).toString(16);
        }
    },

    /**
     * Animates an element with rotation and scaling
     * @param {string} elementId - The ID of the element to animate
     */
    animate: function(elementId) {
        const element = document.getElementById(elementId);
        if (element) {
            element.style.transition = 'all 0.5s ease';
            element.style.transform = 'rotate(360deg) scale(1.2)';
            setTimeout(() => {
                element.style.transform = 'rotate(0deg) scale(1)';
            }, 500);
        }
    }
};

/**
 * Registers C# callbacks that can be invoked from JavaScript
 * @param {DotNetObjectReference} dotNetRef - Reference to the .NET object
 */
window.registerCSharpCallbacks = function(dotNetRef) {
    const btn = document.getElementById('callCSharpBtn');
    if (btn) {
        btn.disabled = false;
        btn.onclick = async function() {
            try {
                // Call static C# method
                const result = await DotNet.invokeMethodAsync(
                    'BlazorHybridDemo',
                    'GetCurrentTime'
                );
                alert('C# returned: ' + result);

                // Call instance C# method
                const instanceResult = await dotNetRef.invokeMethodAsync(
                    'ProcessData',
                    'Hello from JavaScript'
                );
                console.log('Instance method returned:', instanceResult);
            } catch (error) {
                console.error('Error calling C# method:', error);
                alert('Error: ' + error.message);
            }
        };
    }
};