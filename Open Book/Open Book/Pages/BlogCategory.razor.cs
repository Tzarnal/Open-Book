using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using Markdig;
using System.Linq;
using Open_Book.Services;
using Open_Book.Data;
using System;

namespace Open_Book.Pages
{
    public partial class BlogCategory
    {
        [Parameter]
        public String Category { get; set; }

        [Inject]
        private IBlogService BlogService { get; set; }
    }
}