@extends('z_ki-admin.layout.master')
@section('title', 'Input Masks')
@section('css')

@endsection
@section('main-content')
    <div class="container-fluid">

        <!-- Breadcrumb start -->
        <div class="row m-1">
            <div class="col-12 ">
                <h4 class="main-title">Input Masks</h4>
                <ul class="app-line-breadcrumbs mb-3">
                    <li class="">
                        <a class="f-s-14 f-w-500" href="#">
                      <span>
                        <i class="ph-duotone  ph-cardholder f-s-16"></i>  Forms elements
                      </span>
                        </a>
                    </li>
                    <li class="active">
                        <a class="f-s-14 f-w-500" href="#">Input Masks </a>
                    </li>
                </ul>
            </div>
        </div>
        <!-- Breadcrumb end -->

        <!-- Input Masks start -->
        <div class="row">
            <!-- Date Formatting Input Masks start -->
            <div class="col-12">
                <div class="card">
                    <div class="card-header">
                        <h5>Date Formatting Input Masks</h5>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-6 col-xl-4">
                                <form class="app-form mb-3">
                                    <label class="form-label">Simple Date :</label>
                                    <input class="form-control cleave-input-date" placeholder="date"
                                           type="text">
                                </form>
                            </div>
                            <div class="col-md-6 col-xl-4">
                                <form class="app-form mb-3">
                                    <label class="form-label">Date & Month :</label>
                                    <input class="form-control month-input" placeholder="date & month"
                                           type="text">
                                </form>
                            </div>
                            <div class="col-md-6 col-xl-4">
                                <form class="app-form mb-3">
                                    <label class="form-label">Formatting Date :</label>
                                    <input class="form-control formatting-input"
                                           placeholder="formatting date"
                                           type="text">
                                </form>
                            </div>
                            <div class="col-md-6 col-xl-4">
                                <form class="app-form">
                                    <label class="form-label">Formatting Delimiter :</label>
                                    <input class="form-control formatting-delimter" placeholder="delimter"
                                           type="text">
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Date Formatting Input Masks end -->

            <!-- Time Formatting Input Masks start -->
            <div class="col-12">
                <div class="card">
                    <div class="card-header">
                        <h5>Time Formatting Input Masks</h5>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-6 col-xl-4">
                                <form class="app-form">
                                    <label class="form-label">Simple Time :</label>
                                    <input class="form-control time-input" placeholder="time " type="text">
                                </form>
                            </div>
                            <div class="col-md-6 col-xl-4">
                                <form class="app-form">
                                    <label class="form-label">Minutes & Seconds :</label>
                                    <input class="form-control min-sec-input"
                                           placeholder="minutes & seconds"
                                           type="text">
                                </form>
                            </div>
                            <div class="col-md-6 col-xl-4">
                                <form class="app-form">
                                    <label class="form-label">Hours & Minutes :</label>
                                    <input class="form-control hours-min-input"
                                           placeholder="hours & minutes"
                                           type="text">
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Time Formatting Input Masks end -->

            <!-- Custom Formatting Input Masks start -->
            <div class="col-12">
                <div class="card">
                    <div class="card-header">
                        <h5>Custom Formatting Input Masks</h5>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-6">
                                <form class="app-form mb-3">
                                    <label class="form-label">Simple Contact :</label>
                                    <div class="input-group">
                                                    <span class="input-group-text bg-secondary b-r-left"
                                                          id="basic-addon1">+91</span>
                                        <input class="form-control contact-input" placeholder="xxx-xxx-xxxx"
                                               type="text">
                                    </div>
                                </form>
                            </div>
                            <div class="col-md-6">
                                <form class="app-form mb-3">
                                    <label class="form-label">Formatting Contact :</label>
                                    <div class="input-group">
                                                    <span class="input-group-text bg-secondary b-r-left"
                                                          id="basic-addon12">+91</span>
                                        <input class="form-control formatting-contact"
                                               placeholder="(xxx)(xxx)(xxxx)"
                                               type="text">
                                    </div>
                                </form>
                            </div>
                            <div class="col-md-6">
                                <form class="app-form mb-3">
                                    <label class="form-label">Credit card number Formatting :</label>
                                    <input class="form-control credit-input" placeholder="xxxx xxxxx xxxxxx"
                                           type="text">
                                </form>
                            </div>
                            <div class="col-md-6">
                                <form class="app-form mb-3">
                                    <label class="form-label">Numeral Formatting :</label>
                                    <input class="form-control numeral-input" placeholder="xx,xxx,xx"
                                           type="text">
                                </form>
                            </div>
                            <div class="col-md-6">
                                <form class="app-form mb-3">
                                    <label class="form-label">Price :</label>
                                    <input class="form-control price-input" type="text">
                                </form>
                            </div>
                            <div class="col-md-6">
                                <form class="app-form mb-3">
                                    <label class="form-label">Price Formatting :</label>
                                    <input class="form-control price-formatting" type="text">
                                </form>
                            </div>
                            <div class="col-md-6">
                                <form class="app-form">
                                    <label class="form-label">prefix :</label>
                                    <input class="form-control prefix-input" type="text">
                                </form>
                            </div>
                            <div class="col-md-6">
                                <form class="app-form">
                                    <label class="form-label">Prefix with Delimiters :</label>
                                    <input class="form-control prefix-del-input" type="text">
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Custom Formatting Input Masks end -->

            <div class="col-12">
                <div class="card">
                    <div class="card-body">
                        <p>
                            clever.js is Javascript Plugin for Input Masks for more options please check
                            refer
                            <a class="text-decoration-underline link-primary"
                               href="https://nosir.github.io/cleave.js/">https://nosir.github.io/cleave.js/ </a>
                            And
                            <a class="text-decoration-underline link-primary"
                               href="https://github.com/nosir/cleave.js">https://github.com/nosir/cleave.js</a>
                        </p>
                    </div>
                </div>
            </div>
        </div>
        <!-- Input Masks end -->
    </div>
@endsection

@section('script')


<!--cleave js  -->
<script src="{{asset('assets/vendor/cleavejs/cleave.min.js')}}"></script>

<!-- js -->
<script src="{{asset('assets/js/input_masks.js')}}"></script>
@endsection
