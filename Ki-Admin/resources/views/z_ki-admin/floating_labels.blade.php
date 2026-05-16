@extends('z_ki-admin.layout.master')
@section('title', 'Floating Labels')
@section('css')

@endsection
@section('main-content')
    <div class="container-fluid">
        <!-- Breadcrumb start -->
        <div class="row m-1">
            <div class="col-12 ">
                <h4 class="main-title">Floating Labels</h4>
                <ul class="app-line-breadcrumbs mb-3">
                    <li class="">
                        <a class="f-s-14 f-w-500" href="#">
                      <span>
                        <i class="ph-duotone  ph-cardholder f-s-16"></i>  Forms elements
                      </span>
                        </a>
                    </li>
                    <li class="active">
                        <a class="f-s-14 f-w-500" href="#">Floating Labels</a>
                    </li>
                </ul>
            </div>
        </div>
        <!-- Breadcrumb end -->

        <!-- Floating Labels start -->
        <div class="row">
            <div class="col-md-6">
                <div class="card">
                    <div class="card-header">
                        <h5>Custom Floating labels</h5>
                    </div>
                    <div class="card-body">
                        <div class="app-form">
                            <div class="row">
                                <div class="col-12">
                                    <div class="floating-form mb-3">
                                        <input class="form-control" name="name" placeholder="none"
                                               required type="text">
                                        <label class="form-label">Name</label>
                                    </div>
                                </div>
                                <div class="col-12">
                                    <div class="floating-form">
                                        <input class="form-control" placeholder="password" required
                                               type="password">
                                        <label class="form-label">password</label>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>

            <div class="col-md-6">
                <div class="card">
                    <div class="card-header">
                        <h5>Basic Floating Label</h5>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-12">
                                <form class="app-form">
                                    <div class="form-floating mb-3">
                                        <input class="form-control" id="floatingInput"
                                               placeholder="Email address"
                                               type="email">
                                        <label for="floatingInput">Email address</label>
                                    </div>
                                    <div class="form-floating">
                                        <input class="form-control" id="floatingPassword"
                                               placeholder="Password" type="password">
                                        <label for="floatingPassword">Password</label>
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-6">
                <div class="card">
                    <div class="card-header">
                        <h5>Textareas Floating labels</h5>
                    </div>
                    <div class="card-body">
                        <form class="app-form">
                            <div class="form-floating mb-3">
                                                <textarea class="form-control"
                                                          placeholder="Type a comment here"></textarea>
                                <label>Comments</label>
                            </div>
                            <div class="form-floating mb-3">
                                                <textarea class="form-control"
                                                          placeholder="Type a Massage here"></textarea>
                                <label>Massage</label>
                            </div>
                        </form>
                    </div>
                </div>
            </div>

            <div class="col-md-6">
                <div class="card">
                    <div class="card-header">
                        <h5>Input groups Floating labels</h5>
                    </div>
                    <div class="card-body">
                        <div class="col-12">
                            <form class="app-form">
                                <div class="input-group mb-3">
                                    <span class="input-group-text b-r-left">@</span>
                                    <div class="form-floating">
                                        <input class="form-control b-r-right" id="floatingInputGroup1"
                                               placeholder="Username"
                                               type="text">
                                        <label for="floatingInputGroup1">Username</label>
                                    </div>
                                </div>
                                <div class="input-group mb-3">
                                    <span class="input-group-text b-r-left">@</span>
                                    <div class="form-floating">
                                        <input class="form-control b-r-right" placeholder="Email Address"
                                               type="text">
                                        <label for="floatingInputGroup1">Email Address</label>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-6">
                <div class="card">
                    <div class="card-header">
                        <h5>Readonly plaintext Floating labels</h5>
                    </div>
                    <div class="card-body">
                        <div class="col-12">
                            <form class="app-form">
                                <div class="form-floating mb-3">
                                    <input class="form-control-plaintext" id="floatingEmptyPlaintextInput"
                                           placeholder="name@example.com"
                                           readonly type="email">
                                    <label for="floatingEmptyPlaintextInput">Empty input</label>
                                </div>
                                <div class="form-floating">
                                    <input class="form-control-plaintext" id="floatingPlaintextInput"
                                           placeholder="name@example.com"
                                           readonly type="email"
                                           value="name@example.com">
                                    <label for="floatingPlaintextInput">Input with value</label>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-6">
                <div class="card">
                    <div class="card-header">
                        <h5>Floating Input Value</h5>
                    </div>
                    <div class="card-body">
                        <form class="app-form">
                            <div class="form-floating mb-3">
                                <input class="form-control" id="floatingInputValue"
                                       placeholder="name@example.com"
                                       type="email" value="test@example.com">
                                <label for="floatingInputValue">Input with value</label>
                            </div>
                            <div class="form-floating floating-invalid">
                                <input class="form-control is-invalid pe-4" id="floatingInputInvalid"
                                       placeholder="name@example.com" type="email"
                                       value="test@example.com">
                                <label for="floatingInputInvalid">Invalid input</label>
                            </div>
                        </form>
                    </div>
                </div>
            </div>

            <div class="col-md-6">
                <div class="card">
                    <div class="card-header">
                        <h5>select Floating labels</h5>
                    </div>
                    <div class="card-body">
                        <form class="app-form floating-select">
                            <div class="form-floating mb-3">
                                <select aria-label="Floating label select example" class="form-select"
                                        id="floatingSelect">
                                    <option selected>Open this select menu</option>
                                    <option value="1">One</option>
                                    <option value="2">Two</option>
                                    <option value="3">Three</option>
                                </select>
                                <label for="floatingSelect">Works with selects</label>
                            </div>
                            <div class="form-floatin">
                                <select aria-label="Floating label disabled select example"
                                        class="form-select"
                                        disabled id="floatingSelectDisabled">
                                    <option selected>Open this select menu</option>
                                    <option value="1">One</option>
                                    <option value="2">Two</option>
                                    <option value="3">Three</option>
                                </select>
                            </div>
                        </form>
                    </div>
                </div>
            </div>

            <div class="col-md-6">
                <div class="card">
                    <div class="card-header">
                        <h5>Floating labels Layout </h5>
                    </div>
                    <div class="card-body">
                        <form class="app-form">
                            <div class="row g-2">
                                <div class="col-md">
                                    <div class="form-floating">
                                        <input class="form-control" id="floatingInputGrid"
                                               placeholder="name@example.com"
                                               type="email" value="mdo@example.com">
                                        <label for="floatingInputGrid">Email address</label>
                                    </div>
                                </div>
                                <div class="col-md floating-select">
                                    <div class="form-floating">
                                        <select class="form-select form-select-labels"
                                                id="floatingSelectGrid">
                                            <option selected>Open this select menu</option>
                                            <option value="1">One</option>
                                            <option value="2">Two</option>
                                            <option value="3">Three</option>
                                        </select>
                                        <label for="floatingSelectGrid">Works with selects</label>
                                    </div>
                                </div>
                                <div class="col-12">
                                    <div class="form-floating">
                                        <input class="form-control" id="floatingPassword1"
                                               placeholder="Password"
                                               type="password">
                                        <label for="floatingPassword1">Password</label>
                                    </div>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
            </div>

        </div>
        <!-- Floating Labels end -->
    </div>
@endsection

@section('script')


@endsection
