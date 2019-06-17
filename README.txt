# Cos'è Dari&Dali?
E' la repository del **progetto Unity** del corso di "**Gamification e Game Design**" dell'**Università "La Sapienza" di Roma**, anno accademico 2018/19.

# Come scaricare la repository
E' più semplice a farsi che a dirsi! 😏

1) Un po' di downloads e accounts:
	- Iscriviti o loggati a [GitHub](https://github.com/login) (ci conserveremo tutte le modifiche al codice);
	- Installa l'ultima versione di [Git](https://git-scm.com/download/) (servirà per sincronizzare il lavoro fra GitHub e i tuoi dispositivi);
	Lancia da terminale i comandi:
		``` 
		git config --global user.name "<tuoUsernameSuGitHub>"
		git config --global user.email "<tuaEmailSuGitHub>"
		```
	- Installa l'ultima versione di [Unity](https://unity3d.com/get-unity/download) (preferibilmente tramite Unity Hub, per restare sempre aggiornato);
	- *(opzionale)* Installa l'ultima versione di [Visual Studio Code](https://code.visualstudio.com/Download) (l'editor di codice che mi piace di più, è tipo superveloce e figo);
2) Scarichiamo la repository!
	- Da terminale naviga nella cartella dove vuoi clonare quella del progetto e lancia il comando:
		``` 
		git clone https://github.com/rom42pla/DariAndDali.git 
		```
	- Inserisci le tue credenziali di GitHub.
	
Ecco fatto! 
Nella nuova cartella MadmanProj troverai tutto il materiale che ti serve!

# Come switchare fra le branch
> Use a branch to isolate development work without affecting other branches in the repository. Each repository has one default branch, and can have multiple other branches. You can merge a branch into another branch using a pull request.

- *(opzionale)* Puoi creare una nuova **branch**/**merge request** a partire da una **issue** aperta.
- Entra da terminale nella cartella del progetto e lancia il comando:
	``` 
	git branch -a 
	```
	Ti mostrerà i nomi di tutte le possibili branch attive alle quali puoi switchare.
- Cambia branch con il comando:
	``` 
	git checkout <nomeBranch>
	```
	
# Come salvare il lavoro online
Basta solo qualche riga di codice!

- Puoi **salvare temporaneamente** i dati su cui stai lavorando con:
	``` 
	git stash
	```
	E recuperarli con:
	``` 
	git stash apply
	```
	**_Così non salverai nulla sulla branch! Se vuoi salvare permanentemente il tuo lavoro sulla branch usa il metodo sottostante!_**
- Puoi **committare** il tuo lavoro sulla branch e renderlo visibile agli altri sviluppatori con i comandi:
	``` 
	git add *
	git commit -m "<messaggioRiassuntivoDeiCambiamenti>"
	git push -f
	```
