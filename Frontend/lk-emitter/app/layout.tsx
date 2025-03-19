import { Layout } from "antd";
import "./globals.css";
import { SignalRProvider } from "./components/SignalRContext";
import { Content, Footer, Header } from "antd/es/layout/layout";
import { ClientNavigationMenu } from "./components/ClientNavigationMenu";

const items = [
  { key: 'home', label: 'Главная', href: '/' },
  { key: 'directives', label: 'Запросы/поручения', href: '/directives' }
];

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body>
        <Layout style={{minHeight: "100vh"}}>
            <SignalRProvider>
              <Header>
                <ClientNavigationMenu items={items}/>
              </Header>
              <Content style={{padding: "0 48px"}}>
                  {children}
              </Content>
            </SignalRProvider>
            <Footer style={{textAlign: "center"}}>ЛК эмитента</Footer>
          </Layout>
      </body>
    </html>
  );
}
