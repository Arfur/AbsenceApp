@extends('z_ki-admin.layout.master')
@section('title', 'Gallery')
@section('css')
    <!-- glight css -->
    <link rel="stylesheet" href="{{asset('assets/vendor/glightbox/glightbox.min.css')}}">
@endsection
@section('main-content')
    <div class="container-fluid">
        <!-- Breadcrumb start -->
        <div class="row m-1">
            <div class="col-12 ">
                <h4 class="main-title">Gallery</h4>
                <ul class="app-line-breadcrumbs mb-3">
                    <li class="">
                        <a class="f-s-14 f-w-500" href="#">
                    <span>
                      <i class="ph-duotone  ph-stack f-s-16"></i> Apps
                    </span>
                        </a>
                    </li>
                    <li class="active">
                        <a class="f-s-14 f-w-500" href="#">Gallery</a>
                    </li>
                </ul>
            </div>
        </div>
        <!-- Breadcrumb end -->

        <!-- Gallery start -->
        <div class="row">
            <div class="col-12 gallery-grid-container">
                <div class="row gallery-img ">
                    <div class="col-sm-6 col-lg-4">
                        <div class="imagebox">
                            <a class="glightbox" data-glightbox="type: image; zoomable: true;"
                               href="{{asset('../assets/images/gallary/01.jpg')}}" >
                                <img alt="image" class="img-fluid" src="{{asset('../assets/images/gallary/01.jpg')}}">
                            </a>
                            <div class="caption-content">
                                <p>Simple Image</p>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6 col-lg-4">
                        <div class="imagebox">
                            <a class="glightbox"
                               data-glightbox='title:Description Bottom; description: You can set the position of the description '
                               href="{{asset('../assets/images/gallary/02.jpg')}}">
                                <img alt="image" class="img-fluid" src="{{asset('../assets/images/gallary/02.jpg')}}">
                            </a>
                            <div class="caption-content">
                                <p>Image With Bottom Description</p>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-4">
                        <div class="row">
                            <div class="col-6 col-sm-3 col-lg-6">
                                <div class="imagebox">
                                    <a class="glightbox"
                                       data-glightbox='title:Description Right; description: You can set the position of the description ;descPosition: right;'
                                       href="{{asset('../assets/images/gallary/03.jpg')}}">
                                        <img alt="image" class="img-fluid"
                                             src="{{asset('../assets/images/gallary/03.jpg')}}">
                                    </a>
                                    <div class="caption-content">
                                        <p>Image With Right Description</p>
                                    </div>
                                </div>
                            </div>
                            <div class="col-6 col-sm-3 col-lg-6">
                                <div class="imagebox">
                                    <a class="glightbox"
                                       data-glightbox="title: Description Left;  description: You can set the position of the description; descPosition: left;"
                                       href="{{asset('../assets/images/gallary/04.jpg')}}">
                                        <img alt="image" class="img-fluid"
                                             src="{{asset('../assets/images/gallary/04.jpg')}}">
                                    </a>
                                    <div class="caption-content">
                                        <p>Image With Right Description</p>
                                    </div>
                                </div>
                            </div>
                            <div class="col-6 col-sm-3 col-lg-6">
                                <div class="imagebox">
                                    <a class="glightbox"
                                       data-glightbox="title: Description Left;  description: You can set the position of the description; descPosition: top;"
                                       href="{{asset('../assets/images/gallary/05.jpg')}}">
                                        <img alt="image" class="img-fluid"
                                             src="{{asset('../assets/images/gallary/05.jpg')}}">
                                    </a>
                                    <div class="caption-content">
                                        <p>Image With Top Description</p>
                                    </div>
                                </div>
                            </div>
                            <div class="col-6 col-sm-3 col-lg-6">
                                <div class="imagebox">
                                    <a class="glightbox" data-glightbox="type: image; zoomable: true;"
                                       href="{{asset('../assets/images/gallary/06.jpg')}}">
                                        <img alt="image" class="img-fluid"
                                             src="{{asset('../assets/images/gallary/06.jpg')}}">
                                    </a>
                                    <div class="caption-content">
                                        <p>Simple Image</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-6">
                        <div class="row">
                            <div class="col-6 col-lg-4 ">
                                <div class="imagebox">
                                    <a class="glightbox" data-glightbox="type: image; zoomable: true;"
                                       href="{{asset('../assets/images/gallary/07.jpg')}}">
                                        <img alt="" src="{{asset('../assets/images/gallary/07.jpg')}}" class="img-fluid"> </a>
                                    <div class="caption-content">
                                        <p>Image With Right Description</p>
                                    </div>
                                </div>
                            </div>
                            <div class="col-6 col-lg-4 ">
                                <div class="imagebox">
                                    <a class=" glightbox" data-glightbox="type: image; zoomable: true;"
                                       href="{{asset('../assets/images/gallary/08.jpg')}}"><img alt="" src="{{asset('../assets/images/gallary/08.jpg')}}"  class="img-fluid"> </a>
                                    <div class="caption-content">
                                        <p>Image With Right Description</p>
                                    </div>
                                </div>
                            </div>
                            <div class="col-6 col-lg-4 ">
                                <div class="imagebox">
                                    <a class=" glightbox" data-glightbox="type: image; zoomable: true;"
                                       href="{{asset('../assets/images/gallary/14.jpg')}}"><img alt="" src="{{asset('../assets/images/gallary/14.jpg')}}" class="img-fluid"> </a>
                                    <div class="caption-content">
                                        <p>Image With Right Description</p>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-6">
                                <div class="imagebox">
                                    <a class=" glightbox" data-glightbox="type: image; zoomable: true;"
                                       href="{{asset('../assets/images/gallary/10.jpg')}}"><img alt="" src="{{asset('../assets/images/gallary/10.jpg')}}" class="img-fluid"></a>
                                    <div class="caption-content">
                                        <p>Image With Right Description</p>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-6 ">
                                <div class="imagebox">
                                    <a class=" glightbox" data-glightbox="type: image; zoomable: true;"
                                       href="{{asset('../assets/images/gallary/11.jpg')}}"><img alt="" src="{{asset('../assets/images/gallary/11.jpg')}}" class="img-fluid"> </a>
                                    <div class="caption-content">
                                        <p>Image With Right Description</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-6 col-lg-3">
                        <div class="imagebox">
                            <a class="glightbox" data-glightbox="type: image; zoomable: true;"
                               href="{{asset('../assets/images/gallary/16.jpg')}}"><img alt="" src="{{asset('../assets/images/gallary/16.jpg')}}"  class="img-fluid"> </a>
                            <div class="caption-content">
                                <p>Image With Right Description</p>
                            </div>
                        </div>
                    </div>
                    <div class="col-6 col-lg-3 ">
                        <div class="imagebox">
                            <a class=" glightbox" data-glightbox="type: image; zoomable: true;"
                               href="{{asset('../assets/images/gallary/12.jpg')}}"><img alt="" src="{{asset('../assets/images/gallary/12.jpg')}}"> </a>
                            <div class="caption-content">
                                <p>Image With Right Description</p>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6 col-lg-4">
                        <div class="imagebox">
                            <a class=" glightbox" data-glightbox="type: image; zoomable: true;"
                               href="{{asset('../assets/images/gallary/15.jpg')}}"><img alt="" src="{{asset('../assets/images/gallary/15.jpg')}}"  class="img-fluid"> </a>
                            <div class="caption-content">
                                <p>Image With Right Description</p>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6 col-lg-4 ">
                        <div class="imagebox">
                            <a class=" glightbox" data-glightbox="type: image; zoomable: true;"
                               href="{{asset('../assets/images/gallary/13.jpg')}}"><img alt="" src="{{asset('../assets/images/gallary/13.jpg')}}"> </a>
                            <div class="caption-content">
                                <p>Image With Right Description</p>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6 col-lg-4">
                        <div class="imagebox">
                            <a class=" glightbox" data-glightbox="type: image; zoomable: true;"
                               href="{{asset('../assets/images/gallary/09.jpg')}}"><img alt="" src="{{asset('../assets/images/gallary/09.jpg')}}"> </a>
                            <div class="caption-content">
                                <p>Image With Right Description</p>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
        <!-- Gallery end -->
    </div>
@endsection

@section('script')

<!-- Glight js -->
<script src="{{asset('assets/vendor/glightbox/glightbox.min.js')}}"></script>
<script src="{{asset('assets/vendor/masonry/masonry.pkgd.min.js')}}"></script>

<!-- js -->
<script src="{{asset('assets/js/glightbox.js')}}"></script>

@endsection
