# Captain-DaddyBeard
A multimodal game playable on PC and Android simultaneously !

FAQ :

Q : Le réseau wifi n'est pas créé, et le message "Wifi hotspot creation skipped" apparaît
R : Cocher la case "Create wifi hotspot" du Service Connector. Ne pas créer le réseau wifi sert à débugger
plus vite, en utilisant un gestionnaire de réseau wifi comme Virtual Router, qui le fait pour nous de manière
permanente.

Q : Le RPCWrapper semble ne pas fonctionner, mais aucune erreur ne s'affiche
R : Décocher la case "Ignore errors" du RPCWrapper. Cette option permet de cacher les RPC qui échouent lors
des chargements de scènes (la tablette ayant du retard sur le PC et continuant à envoyer de RPC qui n'existent
déjà plus sur le PC).

Bon, j'en avais plein d'autres pendant que je codais, mais maintenant j'ai oublié -_-
