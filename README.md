# anki-editor
Simple editor with script support for Anki decks in the CrowdAnki format.

It simplifies editing and adding cards to decks where some fields can be filled with computed values or values that could be looked up in a dictionary.

Export your deck with the [CrowdAnki](https://github.com/Stvad/CrowdAnki) plugin to start editing. (Make backups of your decks before reimporting edits!)

## Overview
- Adding / Deleting cards
- Editing fields of cards
- Preview field with rendered HTML
- Adding / Removing tags
- **Auto removal of leech tags for sharing the deck**
- Re-adding of removed leech tags
- **Automatic switching the keyboard layout based on the selected field**
- **Scripts** (taking values from one field and store the processed value in the same or in another field)

## Currently available scripts
- Cloning values
- Scripts useful for Japanese
  - Inserting Furigana
  - Dictionary form lookup (for verbs)
  - Word type lookup (type of adjective, verb, ...)

## Found a bug?
Have you found a bug or have an idea for an enhancement?
Create an [issue](https://github.com/ssauermann/anki-editor/issues/new).
