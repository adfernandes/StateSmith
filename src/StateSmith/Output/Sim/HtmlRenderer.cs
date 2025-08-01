using System.Text;

namespace StateSmith.Output.Sim;

public class HtmlRenderer
{
    public static void Render(StringBuilder stringBuilder, string smName, string mermaidCode, string jsCode, string diagramEventNamesArray, string stateEventsMapping)
    {
        // Now that we are working inside the StateSmith project, we need to restrict ourselves to dotnet 6 features.
        // We can't use """raw strings""" anymore so we do manual string interpolation below string.
        // Also, in the below string, we have to use `""` to escape double quotes. I miss raw strings already...
        string htmlTemplate = @"<!-- 
  -- This file was generated by StateSmith.
  -- Note! The generated state machine code in this file has been specially instrumented to support simulator features.
  -- Regular generated javascript state machine code is smaller and simpler.
  -->
<html>
  <head>
    <link rel='icon' type='image/png' href='https://statesmith.github.io/favicon.png'>
    <link rel='stylesheet' href='https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined'>
    <style>
      body {
        display: flex;
        flex-direction: row;
        margin: 0px;
      }

      /* Fix for mermaid content requiring scroll bars https://github.com/StateSmith/StateSmith/issues/288 */
      pre.mermaid {
        margin: 0px;
      }

      .wrapper {
        height: 100vh;
        width: 100vw;
        display: flex;
      }

      .pane {
        padding: 1em;
        min-width: 200px;
      }

      .titlebar-icon {
        font-family: 'Material Symbols Outlined', sans-serif;
        font-size: 16px;
        color: #777;
        border-radius: 5px;
      }

      .gutter {
        width: 10px;
        height: 100%;
        background: #ccc;
        position: absolute;
        top: 0;
        left: 0;
        cursor: col-resize;
      }

      .main {
        flex: 1;
        overflow: auto;
        padding: 10px;
      }

      .sidebar {
        width: 300px;
        padding-top: 0px;
        position: relative;
        background-color: #f0f0f0;
        border-left: 1px solid #ccc;
        display: flex;
        flex-direction: column;
      }

      #buttons {
        display: flex;
        flex-direction: column;
      }

      .titlebar {
        background-color: #ddd;
        border-bottom: 1px solid #ccc;
        font-weight: bold;
        padding: 5px;
        display: flex;
      }

      .console {
        border-collapse: collapse;
        margin-top: 10px;
        width: 100%;
      }

      table.console td.timestamp {
        display: none;
      }

      table.console.timestamps td.timestamp {
        display: table-cell;
      }

      table.console td {
          color: rgba(0, 0, 0, 0.7);
      }

      table.console td .dispatched {
          font-weight: bold;
          color: rgba(0, 0, 0, 1);
      }

      table.console tr:has(+tr td .dispatched) {
          border-bottom: 0px;
      }

      table.console tr:has(+tr td .dispatched) td {
          padding-bottom: 25px;
      }

      .console th {
        background-color: #f0f0f0;
        border-bottom: 1px solid #ccc;
        font-weight: normal;
        padding: 5px;
        text-align: left;
      }

      .console tbody {
        font-family: monospace;
      }

      .console tr {
        border-bottom: 1px solid #ccc;
      }

      .console td {
        padding: 5px;
      }
  
      .console td.timestamp {
        font-size: small;
      }

      .history {
        margin-top: 30px;       
        display: flex;
        overflow: auto;    
        flex-direction: column-reverse;
      }

      .console tr:last-child td {
        border-bottom: none;
      }

      .dispatched {
        font-weight: bold;
      }

      .dispatched > .trigger {
        border: 1px solid #000;
        border-radius: 4px;
        padding: 2px 10px 2px 10px;
      }

      button {
        margin: 5px;
      }

      button.event-button {
        transition: opacity 0.3s ease, background-color 0.3s ease;
        opacity: 1;
        background-color: #007bff;
        color: white;
        cursor: pointer;
      }

      button.event-button:disabled {
        opacity: 0.4;
        background-color: #f0f0f0;
        color: #999;
        cursor: not-allowed;
      }

      button.event-button:not(:disabled):hover {
        background-color: #0056b3;
      }

      /* Style for hiding irrelevant events */
      button.event-button.hidden {
        display: none;
      }



      /* ----------------------------- Dropdown related start ----------------------------- */
      
      .dropdown-button {
        border: none;
        cursor: pointer;
      }
      
      .dropdown-button:hover, .dropdown-button:focus {
        background-color: #f1f1f1;
      }
      
      .dropdown {
        position: relative;
        display: inline-block;
        margin-left: auto;
      }
      
      .dropdown-content {
        display: none;
        position: absolute;
        right: 0;
        background-color: #f1f1f1;
        min-width: 250px;
        overflow: auto;
        box-shadow: 0px 8px 16px 0px rgba(0,0,0,0.2);
        z-index: 1;
      }
      
      .dropdown-content .dropdown-item {
        display: block;
        padding: 12px 16px;
        font-weight: normal;
      }

      .dropdown-content .dropdown-item:hover {
        background-color: #ddd;
        cursor: pointer;
      }

      .show {
        display: block;
      }

      /* ----------------------------- Dropdown related end ----------------------------- */



      .transition.active {
        stroke: #fff5ad !important;
        stroke-width: 5px !important;
        filter: drop-shadow( 3px 3px 2px rgba(0, 0, 0, .7));
      }

      .statediagram-state.active > * {
        fill: #fff5ad !important;
        stroke-width: 2px !important;
      }

    </style>
  </head>

  <body>
    <div class=""wrapper"">
    <div class=""pane main"">
        <pre class=""mermaid"">
{{mermaidCode}}
        </pre>
    </div>

    <div class=""pane sidebar"">
      <div id=""buttons"">
        <div class=""titlebar"">Events
          <div class='dropdown'>
            <span id='dropdown-button' class='titlebar-icon dropdown-button'>settings</span>
            <div id='myDropdown' class='dropdown-content'>
              <label class='dropdown-item' for='hideIrrelevantEvents'
                title='When enabled, event dispatching buttons will be hidden if the current active state(s) ignore the event.'>
                <input type='checkbox' id='hideIrrelevantEvents' name='hideIrrelevantEvents'>
                Hide ignored event buttons
              </label>
              <label class='dropdown-item' for='timestamps'
                title='Controls whether timestamps are shown along side event dispatches.'>
                <input type='checkbox' id='timestamps' name='timestamps'>
                Timestamps
              </label>
            </div>
          </div>
        </div>
      </div>
    
      <div class=""history"">
        <table class=""console"">
          <tbody>
          </tbody>
        </table>
      </div>
    
      <div class=""gutter""></div>
    </div>
    </div>

<script>
{{jsCode}}
</script>

    <script type=""module"">
        import mermaid from 'https://cdn.jsdelivr.net/npm/mermaid@10/dist/mermaid.esm.min.mjs';
        import svgPanZoom from 'https://cdn.jsdelivr.net/npm/svg-pan-zoom@3.6.1/+esm' ;
        mermaid.initialize({ startOnLoad: false });
        await mermaid.run();

        // svg-pan-zoom doesn't like the mermaid viewbox
        document.querySelector('svg').removeAttribute('viewBox');
        document.querySelector('svg').setAttribute('width', '100%');
        document.querySelector('svg').setAttribute('height', '100%');
        document.querySelector('svg').style[""max-width""] = '';

        // don't scale the arrow when we scale the transition edge
        document.querySelectorAll('g defs marker[id$=barbEnd]').forEach(marker => {
            marker.setAttribute('markerUnits', 'userSpaceOnUse');
        });

        // https://github.com/StateSmith/StateSmith/issues/404
        // https://github.com/StateSmith/StateSmith/issues/294
        // rewrite $initial_state to a black circle
        document.querySelectorAll('g[data-id*=""(InitialState)""]').forEach(g=> {
          g.innerHTML = '<circle transform=""translate(0,3)"" height=""14"" width=""14"" r=""14"" class=""state - start""></circle>';
        })

        var panZoom = window.panZoom = svgPanZoom(document.querySelector('svg'), {
            zoomEnabled: true,
            controlIconsEnabled: true,
            fit: true,
            center: true
        });

        const diagramEventNamesArray = {{diagramEventNamesArray}};

        // Mapping from state to available events
        const stateEventsMapping = {{stateEventsMapping}};

        // Get page element references
        const leftPane = document.querySelector("".main"");
        const rightPane = document.querySelector("".sidebar"");
        const gutter = document.querySelector("".gutter"");

        // Function to resize panes
        function resizer(e) {          
          window.addEventListener('mousemove', mousemove);
          window.addEventListener('mouseup', mouseup);          
          let prevX = e.x;
          const rightPanel = rightPane.getBoundingClientRect();
                    
          function mousemove(e) {
            let newX = prevX - e.x;
            rightPane.style.width = rightPanel.width + newX + 'px';
            window.panZoom.resize();
            window.panZoom.fit();
            window.panZoom.center();
          }
          
          function mouseup() {
            window.removeEventListener('mousemove', mousemove);
            window.removeEventListener('mouseup', mouseup);
          }                  
        }

        // Add mouse down event listener for the resizer
        gutter.addEventListener('mousedown', resizer);



        //------------------- drop down functionality start -------------------
        const dropdownButton = document.getElementById('dropdown-button');
        const dropdownDiv = document.getElementById('myDropdown');

        dropdownButton.addEventListener('click', toggleDropdown);

        /* When the user clicks on the button, 
        toggle between hiding and showing the dropdown content */
        function toggleDropdown(event) {
          dropdownDiv.classList.toggle('show');
          event.stopPropagation(); // Prevent click from causing the window click handler to close the dropdown
        }

        // Close the dropdown if the user clicks outside of it
        window.onclick = function(event) {
          const isClickedOutsideDropdownDiv = !dropdownDiv.contains(event.target);

          if (isClickedOutsideDropdownDiv) {
            dropdownDiv.classList.remove('show');
          }
        }

        // Close the dropdown if the user presses Escape
        document.addEventListener('keydown', function(event) {
          if (event.key === 'Escape') {
            dropdownDiv.classList.remove('show');
          }
        });

        // Set the state of the timestamp checkbox
        document.getElementById('timestamps').checked = document.querySelector('table.console').classList.contains('timestamps');
        document.getElementById('timestamps').addEventListener('change', function() {
          if(this.checked) {
            document.querySelector('table.console').classList.add('timestamps');
          } else {
            document.querySelector('table.console').classList.remove('timestamps');
          }
        });

        // Set up hide irrelevant events checkbox state and event listener
        document.getElementById('hideIrrelevantEvents').addEventListener('change', function() {
          // When checkbox state changes, only update button visibility, not availability
          updateButtonVisibility();
        });

        //------------------- drop down functionality end -------------------



        // Convert a date to a string in the format HH:MM:SS.sss
        function formatTime(date) {
            return date.getHours().toString().padStart(2, '0') + ':' +
                date.getMinutes().toString().padStart(2, '0') + ':' +
                date.getSeconds().toString().padStart(2, '0') + '.' +
                date.getMilliseconds().toString().padStart(3, '0');
        }

        // Add a row to the history table.
        function addHistoryRow(time, event, html = false) {
            var row = document.createElement('tr');
            var timeCell = document.createElement('td');
            timeCell.innerText = formatTime(time);
            timeCell.classList.add('timestamp');
            var eventCell = document.createElement('td');

            if(html) {
              eventCell.innerHTML = event;
            } else {
              eventCell.innerText = event;
            }

            row.appendChild(timeCell);
            row.appendChild(eventCell);
            document.querySelector('tbody').appendChild(row);
        }

        var sm = new {{smName}}();

        // prompt the user to evaluate guards manually
        sm.evaluateGuard = (vertexName, behaviorUml) => {
            return confirm(`Evaluate guard for\n${vertexName} behavior:\n${behaviorUml}.\n\nPress 'OK' to evaluate guard as true and 'Cancel' to evaluate it as false.`);
        }; 

        const highlightedEdges = new Set();
        function highlightEdge(edgeId) {
            var edge = document.getElementById(edgeId);
            if (edge) {
              edge.classList.add('active');
              highlightedEdges.add(edge);
            }
        }

        function clearHighlightedEdges() {
            for (const edge of highlightedEdges) {
              edge.classList.remove('active');
              const showOldTraversal = false;
              if (showOldTraversal) {
                  // shows that the edge was traversed. Optional, but kinda nice.
                  edge.style.stroke = 'green';
              }
            }
            highlightedEdges.clear();
        }

        // Function to update event button states (availability and visibility)
        function updateEventButtonStates(currentStateName) {
            const availableEvents = stateEventsMapping[currentStateName] || [];
            
            diagramEventNamesArray.forEach(eventName => {
                const button = document.getElementById('button_' + eventName);
                if (button) {
                    const isAvailable = availableEvents.includes(eventName);
                    
                    // Only set disabled property, CSS :disabled pseudo-class handles styling
                    button.disabled = !isAvailable;
                }
            });
            
            // Update visibility based on checkbox state
            updateButtonVisibility();
        }

        // Function to update button visibility based on Hide Unused checkbox
        function updateButtonVisibility() {
            const hideIrrelevantEvents = document.getElementById('hideIrrelevantEvents').checked;
            
            diagramEventNamesArray.forEach(eventName => {
                const button = document.getElementById('button_' + eventName);
                if (button) {
                    // Toggle hidden class based on checkbox state and button disabled state
                    button.classList.toggle('hidden', hideIrrelevantEvents && button.disabled);
                }
            });
        }

        // The simulator uses a tracer callback to perform operations such as 
        // state highlighting and logging. You do not need this functionality
        // when using {{smName}}.js in your own applications, although you may
        // choose to implement a tracer for debugging purposes.
        sm.tracer = {
            enterState: (mermaidName) => {
                var e = document.querySelector('g[data-id=' + mermaidName + ']');
                if(e) {
                  e.classList.add('active');
                  panOnScreen(e);
                }
                sm.tracer.log('➡️ Entered ' + mermaidName);
                
                // Update event button states
                updateEventButtonStates(mermaidName);
            },
            exitState: (mermaidName) => {
                document.querySelector('g[data-id=' + mermaidName + ']')?.classList.remove('active');
            },
            edgeTransition: (edgeId) => {
                highlightEdge(edgeId);
            },
            log: (message, html=false) => {
                addHistoryRow(new Date(), message, html);
            }
        };

        // Wire up the buttons that dispatch events for the state machine.
        diagramEventNamesArray.forEach(diagramEventName => {
            var button = document.createElement('button');
            button.id = 'button_' + diagramEventName;
            button.className = 'event-button';
            button.innerText = diagramEventName;
            button.addEventListener('click', () => {
                // Only handle click events when button is enabled
                if (!button.disabled) {
                    clearHighlightedEdges();
                    sm.tracer?.log('<span class=""dispatched""><span class=""trigger"">' + diagramEventName + '</span> DISPATCHED</span>', true);
                    const fsmEventName = diagramEventName.toUpperCase();
                    sm.dispatchEvent({{smName}}.EventId[fsmEventName]); 
                }
            });
            document.getElementById('buttons').appendChild(button);
        });

        sm.tracer?.log('<span class=""dispatched"">START</span>', true);
        sm.start(); // This will cause `updateEventButtonStates()` to be called.

        function panOnScreen(element) {
          if(!element) return;

          var bounds = element.getBoundingClientRect();
          if(bounds.x<0 || bounds.y<0) {
              var x = Math.max(0, -bounds.x + 20);
              var y = Math.max(0, -bounds.y + 20);
              window.panZoom.panBy({x: x, y: y});
          }
          var panebounds = document.querySelector('svg').getBoundingClientRect();
          if(bounds.x>panebounds.width || bounds.y>panebounds.height) {
              var x = Math.min(0, panebounds.width - bounds.x - bounds.width - 20);
              var y = Math.min(0, panebounds.height - bounds.y - bounds.height - 20);
              window.panZoom.panBy({x: x, y: y});
          }
        }
    </script>


  </body>
</html>";
        htmlTemplate = htmlTemplate.Replace("{{mermaidCode}}", mermaidCode);
        htmlTemplate = htmlTemplate.Replace("{{jsCode}}", jsCode);
        htmlTemplate = htmlTemplate.Replace("{{smName}}", smName);
        htmlTemplate = htmlTemplate.Replace("{{diagramEventNamesArray}}", diagramEventNamesArray);
        htmlTemplate = htmlTemplate.Replace("{{stateEventsMapping}}", stateEventsMapping);
        stringBuilder.AppendLine(htmlTemplate);
    }
}
