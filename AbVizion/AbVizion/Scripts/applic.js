function ViewModel() {
    var self = this;

    var tokenKey = 'accessToken';

    self.result = ko.observable();
    self.user = ko.observable();

    self.registerEmail = ko.observable();
    self.registerPassword = ko.observable();
    self.registerPassword2 = ko.observable();
    self.userRoles = ["User", "Developer"]

    self.userRole = ko.observable("User");

    self.loginEmail = ko.observable();
    self.loginPassword = ko.observable();

    self.error = ko.observable();

    function showError(jqXHR) {
        self.result(jqXHR.status + ': ' + jqXHR.statusText);
    }

    self.developers = ko.observableArray();
    self.typesComplexity = ["Easy", "Intermediate", "Difficult"]
    self.userName = ko.observable();

    self.newTask = {
        name: ko.observable(),
        description: ko.observable(),
        complexity: ko.observable(),
        applicationUserId: ko.observable()
    }

    self.tasks = ko.observableArray();
    self.task = ko.observable();

    function getAllTasks() {
        var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        $.ajax({
            type: 'GET',
            url: tasksUri,
            headers: headers
        }).done(function (data) {
            self.tasks(data);
        }).fail(showError);

      
    };

    //self.CssBind = ko.computed(function () {
    //        var CssBind = '';
    //        if (ko.unwrap(getComplexity()) === 'Easy') {
    //            CssBind = "green";
    //        } else if (getComplexity() === 'Intermediate') {
    //            CssBind = 'yellow';
    //        }
    //        else if (getComplexity() === 'Difficult') {
    //            CssBind = 'red';
    //        }
    //        return CssBind;
    //});

    self.detail = ko.observable();

    self.getTaskDetail = function (item) {
        ajaxHelper(tasksUri + item.Id, 'GET').done(function (data) {
            self.detail(data);
        });
    }

    self.complexity = ko.observable();

    function getComplexity(task) {
        ajaxHelper(taskItem, 'GET').done(function (data) { self.complexity(data); });
    };

    var tasksUri = '/api/Tasks/';
    var commentsUri = '/api/ comments/';
    var usersInRole = '/api/Account/Developers';
    var userId = '/api/Account/UserId';
    var taskItem = '/api/Tasks/Complexity';

    function ajaxHelper(uri, method, data) {
        self.error(''); // Clear error message
        return $.ajax({
            type: method,
            url: uri,
            dataType: 'json',
            contentType: 'application/json',
            data: data ? JSON.stringify(data) : null
        }).fail(function (jqXHR, textStatus, errorThrown) {
            self.error(errorThrown);
        });
    }

    function getDevelopers() {
        ajaxHelper(usersInRole, 'GET').done(function (data) {
            self.developers(data);
        });
    }

    getAllTasks();
    getDevelopers();

    self.addTask = function () {

        var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        var task = {
            Name: self.newTask.name(),
            Description: self.newTask.description(),
            Complexity: self.newTask.complexity(),
            ApplicationUserId: self.newTask.applicationUserId()
        };

        $.ajax({
            type: 'POST',
            url: tasksUri,
            headers: headers
        }).done(function (task) {
            self.tasks.push(task);
        }).fail(showError);      
    };

    //function getUserName() {
    //    ajaxHelper(usersInRole, 'GET').done(function (data) {
    //        self.developers(data);
    //    });

    self.register = function () {
        self.result('');

        var data = {
            Email: self.registerEmail(),
            Password: self.registerPassword(),
            ConfirmPassword: self.registerPassword2(),
            Role: self.userRole()
        };

        $.ajax({
            type: 'POST',
            url: '/api/Account/Register',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data)
        }).done(function (data) {
            self.result("Done!");
        }).fail(showError);
    }

    self.login = function () {
        self.result('');

        var loginData = {
            grant_type: 'password',
            username: self.loginEmail(),
            password: self.loginPassword()
        };

        $.ajax({
            type: 'POST',
            url: '/Token',
            data: loginData
        }).done(function (data) {
            self.user(data.userName);
            // Cache the access token in session storage.
            sessionStorage.setItem(tokenKey, data.access_token);
        }).fail(showError);
    }

    self.logout = function () {
        self.user('');
        sessionStorage.removeItem(tokenKey)
    }
}

var app = new ViewModel();
ko.applyBindings(app);