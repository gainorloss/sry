import React, { useState } from 'react';
import {
  SearchOutlined,
  PlusCircleFilled,
  InfoCircleFilled,
  QuestionCircleFilled,
  GithubFilled
} from '@ant-design/icons';
import { StyleProvider, legacyLogicalPropertiesTransformer, px2remTransformer } from '@ant-design/cssinjs';
import { ConfigProvider, theme, Input } from 'antd';

import { Outlet, useNavigate, useNavigation } from 'react-router-dom';
import logo from './logo.svg';
import zhCN from 'antd/locale/zh_CN';
import { ProLayout, MenuDataItem, SettingDrawer, ProSettings, PageContainer } from '@ant-design/pro-components';
import { menus } from './config/menus';
const px2rem = px2remTransformer({
  // rootValue: 19, // 32px = 1rem; @default 16
  mediaQuery: true
});

const loopMenuItem = (menus: any[]): MenuDataItem[] =>
  menus.map(({ title, key, children }) => ({
    name: title,
    path: key,
    children: children && loopMenuItem(children),
  }));
const App: React.FC = () => {
  const [settings, setSetting] = useState<Partial<ProSettings> | undefined>({
    layout: 'mix',
  });
  const {
    token: { colorBgContainer },
  } = theme.useToken();
  const navigate = useNavigate();
  console.log(process.env);

  return (<ConfigProvider locale={zhCN}
    theme={{
      token: {
        // Seed Token，影响范围大
        // colorPrimary: '#00b96b',
        // borderRadius: 2,

        // 派生变量，影响范围小
        // colorBgContainer: '#f6ffed',
      },
    }}
  >
    <StyleProvider hashPriority="high" transformers={[legacyLogicalPropertiesTransformer, px2rem]}>
      <ProLayout
        title={process.env.REACT_APP_NAME}
        logo={logo}
        location={{
          pathname: '/admin/process/edit/123',
        }}
        // layout="mix"
        ErrorBoundary={false}
        avatarProps={{
          src: 'https://gw.alipayobjects.com/zos/antfincdn/efFD%24IOql2/weixintupian_20170331104822.jpg',
          size: 'small',
          title: '七妮妮',
        }}
        actionsRender={(props) => {
          if (props.isMobile) return [];
          return [
            props.layout !== 'side' ? (
              <div
                key="SearchOutlined"
                aria-hidden
                style={{
                  display: 'flex',
                  alignItems: 'center',
                  marginInlineEnd: 24,
                }}
                onMouseDown={(e) => {
                  e.stopPropagation();
                  e.preventDefault();
                }}
              >
                <Input
                  style={{
                    borderRadius: 4,
                    marginInlineEnd: 12,
                    backgroundColor: 'rgba(0,0,0,0.03)',
                  }}
                  prefix={
                    <SearchOutlined
                      style={{
                        color: 'rgba(0, 0, 0, 0.15)',
                      }}
                    />
                  }
                  placeholder="搜索方案"
                  bordered={false}
                />
                <PlusCircleFilled
                  style={{
                    color: 'var(--ant-primary-color)',
                    fontSize: 24,
                  }}
                />
              </div>
            ) : undefined,
            <InfoCircleFilled key="InfoCircleFilled" />,
            <QuestionCircleFilled key="QuestionCircleFilled" />,
            <GithubFilled key="GithubFilled" />,
          ];
        }}
        // route={{routes:routes}}
        menu={{ request: async () => loopMenuItem(menus) }}
        menuItemRender={(item: MenuDataItem, dom) => {
          return (
            <div
              onClick={() => {
                if (!item.path) {
                  console.error(`menu route [${item.path}]:not found`);
                  return;
                }
                navigate(item.path);
              }}
            >
              {dom}
            </div>
          )
        }}
        menuFooterRender={(props) => {
          if (props?.collapsed) return undefined;
          return (
            <div
              style={{
                textAlign: 'center',
                paddingBlockStart: 12,
              }}
            >
              <div>© 2023.8 Powered</div>
              <div>by Ant Design + .NET Core</div>
            </div>
          );
        }}
        {...settings}
      >
        <Outlet />
      </ProLayout>
      <SettingDrawer
        // pathname={pathname}
        enableDarkTheme
        getContainer={() => document.getElementById('test-pro-layout')}
        settings={settings}
        onSettingChange={(changeSetting) => {
          setSetting(changeSetting);
        }}
        disableUrlParams={false}
      />
    </StyleProvider>
  </ConfigProvider >
  )
};
export default App;
