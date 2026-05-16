@extends('z_ki-admin.layout.master')
@section('title', 'Widget')
@section('css')
    <!-- apexcharts css-->
    <link rel="stylesheet" type="text/css" href="{{asset('assets/vendor/apexcharts/apexcharts.css')}}">
@endsection
@section('main-content')
    <div class="container-fluid">

        <!-- Breadcrumb start -->
        <div class="row m-1">
            <div class="col-12 ">
                <h4 class="main-title">Widget</h4>
                <ul class="app-line-breadcrumbs mb-3">
                    <li class="">
                        <a class="f-s-14 f-w-500" href="#">
                      <span>
                        <i class="ph-duotone  ph-squares-four f-s-16"></i> Widget
                      </span>
                        </a>
                    </li>
                </ul>
            </div>
        </div>
        <!-- Breadcrumb end -->

        <div class="row widget-container">

            <div class="col-sm-6 col-lg-4 col-xxl-3">
                <div class="card bg-primary">
                    <div class="card-body">
                        <div class="d-flex align-items-center justify-content-between">
                            <h5 class="text-white">Profit Overview</h5>

                        </div>
                        <div>
                            <div id="profitOverview"></div>
                        </div>
                    </div>
                </div>
                <div class="alert  alert-dismissible project-alert" role="alert">
                    <div class="d-flex justify-content-between">
                        <p class="mb-0 z-1 txt-ellipsis-2">
                            🚀 Welcome! Keep track of your projects efficiently.
                        </p>
                        <button aria-label="Close" class="btn-close " data-bs-dismiss="alert"
                                type="button"></button>
                    </div>
                    <div class="progress-bar bg-primary"></div>
                </div>
            </div>

            <div class="col-sm-6 col-lg-4 col-xxl-3">
                <div class="card">
                    <div class="card-body">
                        <div class="d-flex justify-content-between align-items-center">
                            <h6 class="mb-0 txt-ellipsis-1">Yearly Earning</h6>

                            <form class="app-form">
                                <select aria-label="Default select example"
                                        class="form-select custom-form-select">
                                    <option selected="">Jan</option>
                                    <option value="1">Feb</option>
                                    <option value="2">Mar</option>
                                    <option value="3">...</option>
                                    <option value="4">Dec</option>
                                </select>
                            </form>
                        </div>
                        <div id="audienceChart"></div>
                        <ul class="">
                            <li class="py-1 d-flex align-items-center justify-content-between">
                                <p class="mb-0 txt-ellipsis-1">
                                    <i class="ti ti-circle-filled text-primary f-s-10"></i> 19-20 years
                                </p>
                                <p class="text-secondary txt-ellipsis-1 mb-0 flex-grow-1 mx-2"> ------------------------ </p>
                                <span>68%</span>
                            </li>
                            <li class="py-1 d-flex align-items-center justify-content-between">
                                <p class="mb-0 txt-ellipsis-1"><i class="ti ti-circle-filled text-primary-800 f-s-10"></i> 20-21 years</p>
                                <p class="text-secondary txt-ellipsis-1 mb-0 flex-grow-1 mx-2"> ------------------------ </p>
                                <span>58%</span>
                            </li>
                            <li class="py-1 d-flex align-items-center justify-content-between">
                                <p class="mb-0 txt-ellipsis-1"><i class="ti ti-circle-filled text-primary-600 f-s-10"></i> 21-22 years</p>
                                <p class="text-secondary txt-ellipsis-1 mb-0 flex-grow-1 mx-2"> ------------------------ </p>
                                <span>78%</span>
                            </li>
                            <li class="py-1 d-flex align-items-center justify-content-between">
                                <p class="mb-0 txt-ellipsis-1"><i class="ti ti-circle-filled text-primary-400 f-s-10"></i> 22-23 years</p>
                                <p class="text-secondary txt-ellipsis-1 mb-0 flex-grow-1 mx-2"> ----------------------- </p>
                                <span>88%</span>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>

            <div class="col-sm-7 col-lg-6 col-xxl-4 order-1-lg">

                <div class="card project-data-container">
                    <div class="card-body">
                        <div class="d-flex justify-content-between align-items-center">
                            <h6 class="mb-0">Project </h6>

                            <form class="app-form flex-shrink-0">
                                <select aria-label="Default select example" class="form-select custom-form-select ">
                                    <option selected="">Filter</option>
                                    <option value="1">Fashion</option>
                                    <option value="2">Books</option>
                                    <option value="3">Sports</option>
                                    <option value="4">Fitness</option>
                                </select>
                            </form>
                        </div>
                        <div class="row project-row mt-4">
                            <div class="col-sm-4">
                                <div class="project-status-card bg-primary text-center w-100 rounded p-3 shadow">
                                             <span class="bg-white h-45 w-45 d-flex-center b-r-50 status-icon">
                                                <i class="ti ti-clock-hour-5 f-s-20 text-primary"></i>
                                             </span>
                                    <p class="text-white mb-0 txt-ellipsis-1">Running</p>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="project-status-card bg-success text-center w-100 rounded p-3 shadow">
                                            <span class="bg-white h-45 w-45 d-flex-center b-r-50 status-icon">
                                                <i class="ti ti-circle-check f-s-20 text-success"></i>
                                             </span>
                                    <p class="text-white mb-0 txt-ellipsis-1">Completed</p>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="project-status-card bg-danger text-center w-100 rounded p-3 shadow">
                                            <span class="bg-white h-45 w-45 d-flex-center b-r-50 status-icon">
                                                <i class="ti ti-refresh f-s-20 text-danger"></i>
                                             </span>
                                    <p class="text-white mb-0 txt-ellipsis-1">Pending</p>
                                </div>
                            </div>
                        </div>

                        <div class="row mt-3">
                            <div class="col-sm-6">
                                <ul class="">
                                    <li class="d-flex align-items-center justify-content-between">
                                        <div class="bg-danger-200 d-flex-center p-1 w-30 h-30 b-r-8">
                                            <img alt="avatar" class="img-fluid" src="{{asset('assets/images/icons/language/logo1.png')}}">
                                        </div>
                                        <div class="ms-2 flex-grow-1">
                                            <p class="text-dark-800 mb-0 f-w-500 f-s-15 txt-ellipsis-1">New Task Assigned</p>
                                        </div>
                                    </li>
                                    <li class="d-flex align-items-center justify-content-between mt-3">
                                        <div class="bg-success-200 d-flex-center p-1 w-30 h-30 b-r-8">
                                            <img alt="avatar" class="img-fluid" src="{{asset('assets/images/icons/language/logo5.png')}}">
                                        </div>
                                        <div class="ms-2 flex-grow-1">
                                            <p class="text-dark-800 mb-0 f-w-500 f-s-15 txt-ellipsis-1">Database Migration</p>
                                        </div>
                                    </li>
                                    <li class="d-flex align-items-center justify-content-between mt-3">
                                        <div class="bg-info-200 d-flex-center p-1 w-30 h-30 b-r-8">
                                            <img alt="avatar" class="img-fluid" src="{{asset('assets/images/icons/language/logo6.png')}}">
                                        </div>
                                        <div class="ms-2 flex-grow-1">
                                            <p class="text-dark-800 mb-0 f-w-500 f-s-15 txt-ellipsis-1">New Task Assigned</p>
                                        </div>
                                    </li>
                                </ul>
                            </div>
                            <div class="col-sm-6">
                                <ul class="">
                                    <li class="d-flex align-items-center justify-content-between">
                                        <div class="bg-primary-200 d-flex-center p-1 w-30 h-30 b-r-8">
                                            <img alt="avatar" class="img-fluid" src="{{asset('assets/images/icons/language/logo4.png')}}">
                                        </div>
                                        <div class="ms-2 flex-grow-1">
                                            <p class="text-dark-800 mb-0 f-w-500 f-s-15 txt-ellipsis-1">API Development
                                                Phase</p>
                                        </div>
                                    </li>
                                    <li class="d-flex align-items-center justify-content-between mt-3">
                                        <div class="bg-danger-200 d-flex-center p-1 w-30 h-30 b-r-8">
                                            <img alt="avatar" class="img-fluid" src="{{asset('assets/images/icons/language/logo3.png')}}">
                                        </div>
                                        <div class="ms-2 flex-grow-1">
                                            <p class="text-dark-800 mb-0 f-w-500 f-s-15 txt-ellipsis-1">UI/UX Design Update</p>
                                        </div>
                                    </li>
                                    <li class="d-flex align-items-center justify-content-between mt-3">
                                        <div class="bg-info-200 d-flex-center p-1 w-30 h-30 b-r-8">
                                            <img alt="avatar" class="img-fluid" src="{{asset('assets/images/icons/language/logo2.png')}}">
                                        </div>
                                        <div class="ms-2 flex-grow-1">
                                            <p class="text-dark-800 mb-0 f-w-500 f-s-15 txt-ellipsis-1">New Task Assigned</p>
                                        </div>
                                    </li>
                                </ul>
                            </div>
                        </div>

                        <div class="mt-3">
                            <button class="btn btn-primary w-100 btn-sm">View all</button>
                        </div>

                    </div>
                </div>
            </div>

            <div class="col-sm-5 col-lg-4 col-xxl-2">
                    <div class="card service-trial-card">
                        <div class="card-body">
                            <h5 class="text-white f-w-600 mt-3 txt-ellipsis-2"> AI Commerce</h5>
                            <p class="text-white mt-2 txt-ellipsis-2">Smarter Shopping, Faster Growth</p>
                            <div class="mt-4 service-img-box"></div>
                            <div class="mt-4">
                                <button class="btn btn-primary btn-sm w-100 txt-ellipsis-1">Start Free Trial</button>
                            </div>

                        </div>
                    </div>
            </div>

            <div class="col-sm-6 col-lg-4 col-xxl-3">
                <div class="card overflow-hidden">
                    <div class="card-body p-0">
                        <div class="meeting-call-box bg-gradient-mode">
                            <img alt="img"
                                 class="img-fluid position-relative z-1"
                                 src="{{asset('assets/images/dashboard/project/meeting-avtar.png')}}">
                            <img alt="img"
                                 class="img-fluid bg-vector-img"
                                 src="{{asset('assets/images/dashboard/project/bg-round.png')}}">
                            <img alt="img"
                                 class="img-fluid bg-vector-img1"
                                 src="{{asset('assets/images/dashboard/project/bg-round2.png')}}">

                            <div class="meeting-details-box d-flex align-items-center">
                                <div class="h-40 w-40 d-flex-center b-r-50 overflow-hidden bg-dark flex-shrink-0">
                                    <img alt="image" class="img-fluid"
                                         src="{{asset('assets/images/avatar/2.png')}}">
                                </div>
                                <div class="flex-grow-1 ps-2">
                                    <div class="fw-medium txt-ellipsis-1"> Bette Hagenes</div>
                                    <div class="text-muted f-s-12 txt-ellipsis-1">Wed Developer</div>
                                </div>
                                <button class="btn btn-success btn-sm mt-2">Join</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-sm-6 col-xxl-3 order-2-lg">
                <div class="card">
                    <div class="card-body pb-0">
                        <div>
                            <h4 class="text-primary">98.65% <span class="f-s-14 text-dark">Total sale</span></h4>
                        </div>

                        <div id="revenueChart"></div>
                    </div>
                </div>
            </div>

            <div class="col-sm-6 col-lg-4 col-xxl-3">
                <div class="card offer-card-box">
                    <div class="circle-ribbon circle-left ribbon-danger b-4-white">
                        50%
                    </div>

                    <div class="card-body offer-card-body overflow-hidden ">
                        <div>
                            <div class="my-3">
                                <span class="badge text-primary f-s-10 bg-white-500">Clothing</span>
                                <span class="badge text-primary f-s-10 bg-white-500">Toys</span>
                                <span class="badge text-primary f-s-10 bg-white-500">Accessories</span>
                            </div>
                            <h5 class="text-white mt-4">Super <span class="text-bg-primary  p-1 f-s-26 f-w-700 ">Kids’</span> Weekend
                                <br> <span class="text-danger highlight-word p-1">Sale</span></h5>

                        </div>
                        <div>
                            <button class="btn btn-white  f-w-500 w-100 my-2">Shop Now</button>
                            <a class="f-s-12 f-w-500 text-white text-d-underline" href="#">Minimum purchase
                                of $30 required. Online &amp; in-store.</a>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-sm-6 col-lg-4 col-xxl-3">
                <div class="card">
                    <div class="card-body">
                        <div>
                            <h4 class="text-primary">$65,563.24</h4>
                            <p class="mb-0 text-secondary"><span class="text-light-danger">38.3%-</span>
                                Last week</p>
                        </div>
                        <div>
                            <div id="earningChart"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>
@endsection

@section('script')

<!-- apexcharts -->
<script src="{{asset('assets/vendor/apexcharts/apexcharts.min.js')}}"></script>
<script src="{{asset('assets/vendor/apexcharts/column/dayjs.min.js')}}"></script>
<script src="{{asset('assets/vendor/apexcharts/column/quarterOfYear.min.js')}}"></script>

<!--js-->
<script src="{{asset('assets/js/widget.js')}}"></script>
@endsection
