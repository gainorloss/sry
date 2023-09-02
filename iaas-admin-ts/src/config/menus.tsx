const menus = [
    {
      title: '首页', 
      key: '/', 
      icon: 'home', 
      isPublic: true, 
    },
    {
      title: '商品中心',
      key: '/pc',
      icon: 'appstore',
      children: [ 
        {
          title: '品类管理',
          key: '/pc/category',
          icon: 'bars'
        },
        {
          title: '商品管理',
          key: '/pc/spu',
          icon: 'tool'
        },
      ]
    },
  
    {
      title: '用户中心',
      key: '/uc',
      icon: 'user'
    },
    {
      title: '角色管理',
      key: '/role',
      icon: 'safety',
    },
  
    {
      title: '图形图表',
      key: '/charts',
      icon: 'area-chart',
      children: [
        {
          title: '柱形图',
          key: '/charts/bar',
          icon: 'bar-chart'
        },
        {
          title: '折线图',
          key: '/charts/line',
          icon: 'line-chart'
        },
        {
          title: '饼图',
          key: '/charts/pie',
          icon: 'pie-chart'
        },
      ]
    },
    {
      title: '库存中心',
      key: '/sc',
      icon: 'windows',
    },
    {
      title: '订单中心',
      key: '/oc',
      icon: 'windows',
    },  
    {
      title: '登录',
      key: '/login',
      icon: 'windows',
    },
  ]
  
  export {menus} 
  